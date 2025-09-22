using System.Globalization;
using System.Text.RegularExpressions;
using SupportBot.Core.Data;
using SupportBot.Core.Models;

namespace SupportBot.Core.Services;

/// <summary>
/// Lightweight rule based engine that produces confident answers for the most common Allyouplay support scenarios.
/// </summary>
public sealed class IssueResponseGenerator : IResponseGenerator
{
    private static readonly IReadOnlyList<(string Keyword, string GameName)> KnownGameNames = new List<(string, string)>
    {
        ("helldivers", "Helldivers II"),
        ("elden ring", "Elden Ring"),
        ("fifa", "EA SPORTS FC"),
        ("ea sports fc", "EA SPORTS FC"),
        ("call of duty", "Call of Duty"),
        ("cod", "Call of Duty"),
        ("baldur", "Baldur's Gate 3"),
        ("hogwarts", "Hogwarts Legacy"),
        ("diablo", "Diablo IV"),
        ("assassin", "Assassin's Creed"),
        ("cyberpunk", "Cyberpunk 2077"),
        ("gta", "Grand Theft Auto"),
        ("forza", "Forza Horizon"),
        ("minecraft", "Minecraft"),
    };

    private readonly IReadOnlyList<IssueTemplate> _templates;

    public IssueResponseGenerator()
        : this(IssueTemplateProvider.CreateDefaultTemplates())
    {
    }

    public IssueResponseGenerator(IReadOnlyList<IssueTemplate> templates)
    {
        _templates = templates;
    }

    public ChatResponse GenerateResponse(ChatRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Message))
        {
            return new ChatResponse(
                "Size yardımcı olabilmem için yaşadığınız durumu birkaç cümle ile paylaşabilir misiniz?",
                "insufficient-input",
                0,
                Array.Empty<string>(),
                new[]
                {
                    "Ödemenin yapıldığı hesap adı veya sipariş numaranızı paylaşabilir misiniz?"
                },
                Array.Empty<string>());
        }

        var normalized = Normalize(request.Message);
        var rankedTemplate = _templates
            .Select(template => (Template: template, Score: ScoreTemplate(template, normalized)))
            .OrderByDescending(result => result.Score)
            .First();

        var templateToUse = rankedTemplate.Score > 0 ? rankedTemplate.Template : _templates.Last();
        var confidence = CalculateConfidence(rankedTemplate.Score, templateToUse);
        var reply = PersonalizeReply(templateToUse, request, normalized);

        return new ChatResponse(
            reply,
            templateToUse.Category,
            confidence,
            templateToUse.SuggestedActions,
            templateToUse.FollowUpQuestions,
            templateToUse.AdditionalNotes);
    }

    private static string Normalize(string message)
    {
        var lower = message.ToLowerInvariant();
        lower = Regex.Replace(lower, "\\s+", " ");
        return lower.Trim();
    }

    private static double ScoreTemplate(IssueTemplate template, string normalizedMessage)
    {
        double score = 0;
        foreach (var keyword in template.Keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                continue;
            }

            if (normalizedMessage.Contains(keyword, StringComparison.Ordinal))
            {
                score += 1 + Math.Min(keyword.Length / 10.0, 1.5);
            }
        }

        return score;
    }

    private static double CalculateConfidence(double rawScore, IssueTemplate template)
    {
        if (rawScore <= 0)
        {
            return 0.25;
        }

        // Longer templates usually have more keywords, so smooth the score.
        var normalized = rawScore / (template.Keywords.Length + 1);
        normalized = Math.Clamp(normalized, 0.25, 1.0);
        return Math.Round(normalized, 2);
    }

    private static string PersonalizeReply(IssueTemplate template, ChatRequest request, string normalized)
    {
        var reply = template.ReplyTemplate;

        if (reply.Contains("{{gameName}}", StringComparison.Ordinal))
        {
            var gameName = DetectGameName(normalized, request.Message);
            reply = reply.Replace("{{gameName}}", gameName ?? "oyun", StringComparison.Ordinal);
        }

        if (!string.IsNullOrWhiteSpace(request.OrderNumber))
        {
            reply += $" İlgili sipariş numaranız {request.OrderNumber} olarak not edildi.";
        }

        return reply;
    }

    private static string? DetectGameName(string normalizedMessage, string originalMessage)
    {
        foreach (var (keyword, gameName) in KnownGameNames)
        {
            if (normalizedMessage.Contains(keyword, StringComparison.Ordinal))
            {
                return gameName;
            }
        }

        // Attempt to capture sequences like "helldivers aldım" or "elden ring keyi".
        var regexes = new[]
        {
            new Regex("(?<game>[a-z0-9'\\s]{3,})\\s+(?:aldim|aldım|satin|satın|purchase|bought)", RegexOptions.IgnoreCase),
            new Regex("(?<game>[a-z0-9'\\s]{3,})\\s+(?:key|kod|anahtar)", RegexOptions.IgnoreCase),
        };

        foreach (var regex in regexes)
        {
            var match = regex.Match(originalMessage);
            if (match.Success)
            {
                var candidate = match.Groups["game"].Value.Trim();
                if (!string.IsNullOrWhiteSpace(candidate))
                {
                    // Clean up repeated whitespace and capitalise words.
                    candidate = Regex.Replace(candidate, "\\s+", " ");
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(candidate.ToLowerInvariant());
                }
            }
        }

        return null;
    }
}
