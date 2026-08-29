namespace HotelWise.Domain.Interfaces.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.ITokenService.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public interface ITokenService : HotelWise.Core.SDK.Abstractions.ITokenService
    {
    }
}
