using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Helpers.AI;

namespace HotelWise.Domain.Dto.IA.SemanticKernel
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (enums Domain ≠ Core durante migração).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.DTO.PromptMessageVO.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class PromptMessageVO
    {
        public DataVectorVO[] DataContextRag { get; set; }
        public RoleAiPromptsType RoleType { get; set; }
        public string Role => RoleType.GetDescription();
        public string Content { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public int TokenCount
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Content))
                {
                    return TokenCounterHelper.CountTokens(Content);
                }
                if (DataContextRag != null && DataContextRag.Length > 0)
                {
                    TokenCounterHelper.CalculateDataVectorLength(DataContextRag);
                }
                return 0;
            }
        }
        public int ContentLenght { get { return Content.Length; } }
    }

    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.DTO.DataVectorVO.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class DataVectorVO
    {
        public string KeyVector { get; set; } = string.Empty;
        public string DataVector { get; set; } = string.Empty;
    }
}
