namespace HotelWise.Domain.Enuns
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.ETypeDataBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public enum ETypeDataBase
    {
        MSsqlServer = 0,
        Mysql = 1,
        Postgree = 3,
        FireBase = 4,
    }
}
