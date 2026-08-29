using HotelWise.Domain.Interfaces.IA;

namespace HotelWise.Domain.Dto.IA.SemanticKernel
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.DTO.DataVectorBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public abstract class DataVectorBase : HotelWise.Core.SDK.AI.DTO.DataVectorBase, IDataVector
    {
    }
}
