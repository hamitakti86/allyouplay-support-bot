namespace SupportBot.Core.Models;

/// <summary>
/// Represents a known support scenario and the information required to answer it.
/// </summary>
public sealed class IssueTemplate
{
    public required string Category { get; init; }

    public required string Summary { get; init; }

    public required string[] Keywords { get; init; }

    public required string ReplyTemplate { get; init; }

    public required string[] SuggestedActions { get; init; }

    public string[] FollowUpQuestions { get; init; } = Array.Empty<string>();

    public string[] AdditionalNotes { get; init; } = Array.Empty<string>();
}
