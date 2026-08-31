using HotelWise.Core.SDK.AI.Enums;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Resposta do assistente conversacional.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AskAssistantResponse : SchDto.AskAssistantResponse
{
    public new RoleAiPromptsType Role
    {
        get => (RoleAiPromptsType)(int)base.Role;
        set => base.Role = (SchEnums.RoleAiPromptsType)(int)value;
    }
}

/// <summary>
/// Solicitação ao assistente conversacional.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantRequest. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AskAssistantRequest : SchDto.AskAssistantRequest
{
    public new RoleAiPromptsType Role => (RoleAiPromptsType)(int)base.Role;
}
