namespace HotelWise.Domain.Interfaces.Base
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.IEntityBaseLog.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DOMAIN")]
    public interface IEntityBaseLog : HotelWise.Core.SDK.Abstractions.IEntityBaseLog
    {
    }
}
