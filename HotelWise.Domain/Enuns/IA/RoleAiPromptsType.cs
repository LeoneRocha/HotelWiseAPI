using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Enums.RoleAiPromptsType.", error: false, DiagnosticId = "HW_CORE_SDK_AI")]
    public enum RoleAiPromptsType
    {
        [JsonPropertyName("system")] [Description("system")] System = 1,
        [JsonPropertyName("user")] [Description("user")] User = 2,
        [JsonPropertyName("assistant")] [Description("assistant")] Assistant = 3,
        [JsonPropertyName("agent")] [Description("agent")] Agent = 4,
        [JsonPropertyName("Context")] [Description("Context")] Context = 5
    }
}
