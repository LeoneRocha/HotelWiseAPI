using HotelWise.Core.SDK.AI.Enums;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Resposta do assistente conversacional.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantResponse", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantResponse em SmartCoreHub.Core.SDK.")]
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
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantRequest", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantRequest em SmartCoreHub.Core.SDK.")]
public class AskAssistantRequest : SchDto.AskAssistantRequest
{
    /// <summary>Papel do emissor da solicitação.</summary>
    public new RoleAiPromptsType Role => (RoleAiPromptsType)(int)base.Role;
}
