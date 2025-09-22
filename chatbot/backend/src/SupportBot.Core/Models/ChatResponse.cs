namespace SupportBot.Core.Models;

/// <summary>
/// Encapsulates the automatically generated answer for a shopper request.
/// </summary>
/// <param name="Reply">Natural language answer tailored for the shopper.</param>
/// <param name="Category">The detected support scenario.</param>
/// <param name="Confidence">A score between 0 and 1 representing how confident the bot is about the detected scenario.</param>
/// <param name="SuggestedActions">Step-by-step actions that the shopper can follow.</param>
/// <param name="FollowUpQuestions">Questions to collect additional context when needed.</param>
/// <param name="AdditionalNotes">Extra operational notes for the support team.</param>
public sealed record ChatResponse(
    string Reply,
    string Category,
    double Confidence,
    IReadOnlyList<string> SuggestedActions,
    IReadOnlyList<string> FollowUpQuestions,
    IReadOnlyList<string> AdditionalNotes);
