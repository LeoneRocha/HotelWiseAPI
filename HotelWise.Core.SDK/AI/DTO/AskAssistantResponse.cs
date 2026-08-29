using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Resposta do assistente conversacional.
/// </summary>
public class AskAssistantResponse
{
    public RoleAiPromptsType Role { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Solicitação ao assistente conversacional.
/// </summary>
public class AskAssistantRequest
{
    public RoleAiPromptsType Role { get; } = RoleAiPromptsType.User;
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
