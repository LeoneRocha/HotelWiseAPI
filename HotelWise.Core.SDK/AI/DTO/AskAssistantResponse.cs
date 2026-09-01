using HotelWise.Core.SDK.AI.Enums;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Resposta do assistente conversacional.
/// </summary>
public class AskAssistantResponse : SchDto.AskAssistantResponse
{
    /// <summary>Papel do emissor da resposta.</summary>
    public new RoleAiPromptsType Role
    {
        get => (RoleAiPromptsType)(int)base.Role;
        set => base.Role = (SchEnums.RoleAiPromptsType)(int)value;
    }
}

/// <summary>
/// Solicitação ao assistente conversacional.
/// </summary>
public class AskAssistantRequest : SchDto.AskAssistantRequest
{
    /// <summary>Papel do emissor da solicitação.</summary>
    public new RoleAiPromptsType Role => (RoleAiPromptsType)(int)base.Role;
}
