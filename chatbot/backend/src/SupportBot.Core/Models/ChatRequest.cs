namespace SupportBot.Core.Models;

/// <summary>
/// Represents a message sent from the shopper to the support bot.
/// </summary>
/// <param name="Message">The free form text that the customer entered.</param>
/// <param name="OrderNumber">Optional order reference provided by the shopper.</param>
/// <param name="Locale">Preferred language/locale string (e.g. "tr-TR").</param>
public sealed record ChatRequest(string Message, string? OrderNumber = null, string? Locale = null);
