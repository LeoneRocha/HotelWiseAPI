using System.ComponentModel;
using System.Text.Json.Serialization;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Papéis (roles) das mensagens no histórico de prompts de IA.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Enums.RoleAiPromptsType. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public enum RoleAiPromptsType
{
    [JsonPropertyName("system")]
    [Description("system")]
    System = (int)SchEnums.RoleAiPromptsType.System,

    [JsonPropertyName("user")]
    [Description("user")]
    User = (int)SchEnums.RoleAiPromptsType.User,

    [JsonPropertyName("assistant")]
    [Description("assistant")]
    Assistant = (int)SchEnums.RoleAiPromptsType.Assistant,

    [JsonPropertyName("agent")]
    [Description("agent")]
    Agent = (int)SchEnums.RoleAiPromptsType.Agent,

    [JsonPropertyName("Context")]
    [Description("Context")]
    Context = (int)SchEnums.RoleAiPromptsType.Context,
}
