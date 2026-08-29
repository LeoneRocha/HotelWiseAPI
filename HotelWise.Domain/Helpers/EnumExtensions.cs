namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.EnumExtensions.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value) =>
            HotelWise.Core.SDK.Extensions.EnumExtensions.GetDescription(value);
    }
}
