using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Dto.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (enums Domain ≠ Core durante migração).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.DTO.AskAssistantResponse.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class AskAssistantResponse
    {
        public RoleAiPromptsType Role { get; set; }

        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (enums Domain ≠ Core durante migração).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.DTO.AskAssistantRequest.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class AskAssistantRequest
    {
        public RoleAiPromptsType Role { get; } = RoleAiPromptsType.User;
        public string Message { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
