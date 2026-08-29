using System.Globalization;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de data/hora e cultura.
/// </summary>
public static class DataHelper
{
    public static string ConvertSecondsToTimeString(double seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"hh\:mm\:ss");
    }

    private static readonly string dateFormat = "dd/MM/yyyy HH:mm:ss";

    public static string GetDateTimeCustomFormat(DateTime dateInput)
    {
        var cultureInfo = CultureInfo.InvariantCulture;
        var result = dateInput.ToString(dateFormat, cultureInfo);
        return result;
    }

    public static void SetCulture()
    {
        var cultureInfo = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    public static DateTime GetDateTimeNowBrazil()
    {
        DateTime now = DateTime.UtcNow;
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        DateTime brazilTime = TimeZoneInfo.ConvertTimeFromUtc(now, tzi);
        return brazilTime;
    }

    public static DateTime GetDateTimeNowToLog() => GetDateTimeNowBrazil();

    public static DateTime GetDateTimeNowToProcess() => GetDateTimeNowBrazil();

    public static DateTime GetDateTimeNowToPersistData() => GetDateTimeNowBrazil();

    public static DateTime GetDateTimeNow() => DateTime.UtcNow;

    public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        return dateTimeWithTimeZone;
    }
}
