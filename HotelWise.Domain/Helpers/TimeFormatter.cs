namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.TimeFormatter.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class TimeFormatter
    {
        public static string FormatElapsedTime(TimeSpan elapsed) =>
            HotelWise.Core.SDK.Helpers.TimeFormatter.FormatElapsedTime(elapsed);
    }
}
