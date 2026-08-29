namespace HotelWise.Domain.Model
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Domain.EntityBaseWithNameEmail.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DOMAIN")]
    public abstract class EntityBaseWithNameEmail : HotelWise.Core.SDK.Domain.EntityBaseWithNameEmail
    {
    }
}
