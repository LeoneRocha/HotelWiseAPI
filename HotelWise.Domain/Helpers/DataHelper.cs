namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.DataHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class DataHelper
    {
        public static string ConvertSecondsToTimeString(double seconds) =>
            HotelWise.Core.SDK.Helpers.DataHelper.ConvertSecondsToTimeString(seconds);

        public static string GetDateTimeCustomFormat(DateTime dateInput) =>
            HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeCustomFormat(dateInput);

        public static void SetCulture() => HotelWise.Core.SDK.Helpers.DataHelper.SetCulture();

        public static DateTime GetDateTimeNowBrazil() => HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNowBrazil();

        public static DateTime GetDateTimeNowToLog() => HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNowToLog();

        public static DateTime GetDateTimeNowToProcess() => HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNowToProcess();

        public static DateTime GetDateTimeNowToPersistData() => HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNowToPersistData();

        public static DateTime GetDateTimeNow() => HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNow();

        public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId) =>
            HotelWise.Core.SDK.Helpers.DataHelper.ApplyTimeZone(dateTime, timeZoneId);
    }
}
