using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAIInferenceAdapterFactory.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAIInferenceAdapterFactory
    {
        IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType);
    }
}
