using SupportBot.Core.Models;

namespace SupportBot.Core.Services;

/// <summary>
/// Generates human readable answers for shopper chat messages.
/// </summary>
public interface IResponseGenerator
{
    ChatResponse GenerateResponse(ChatRequest request);
}
