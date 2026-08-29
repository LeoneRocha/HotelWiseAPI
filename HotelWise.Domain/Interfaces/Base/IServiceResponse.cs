namespace HotelWise.Domain.Interfaces.Base
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.IServiceResponse<T>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public interface IServiceResponse<T> : HotelWise.Core.SDK.Abstractions.IServiceResponse<T>
    {
    }
}
