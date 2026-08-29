namespace HotelWise.Domain.Interfaces.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAiInferenceConfigBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAiInferenceConfigBase : HotelWise.Core.SDK.AI.Abstractions.IAiInferenceConfigBase
    {
    }
}
