using System.Globalization;

namespace HotelWise.Domain.Helpers
{
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
            var cultureInfo = System.Globalization.CultureInfo.InvariantCulture;
            var result = dateInput.ToString(dateFormat, cultureInfo);
            return result;
        }

        public static void SetCulture()
        {
            // Set the culture to Portuguese (Brazil)
            var cultureInfo = new CultureInfo("pt-BR");

            // Set the culture of the current thread
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            // Set the UI culture of the current thread
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public static DateTime GetDateTimeNowBrazil()
        {
            DateTime now = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime brazilTime = TimeZoneInfo.ConvertTimeFromUtc(now, tzi);
            return brazilTime;
        }

        public static DateTime GetDateTimeNowToLog()
        {
            return GetDateTimeNowBrazil();
        }

        public static DateTime GetDateTimeNowToProcess()
        {
            return GetDateTimeNowBrazil();
        }

        public static DateTime GetDateTimeNowToPersistData()
        {
            return GetDateTimeNowBrazil();
        }

        public static DateTime GetDateTimeNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId)
        {
            // Obter o fuso horário a partir do ID
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Converter a data e hora para o fuso horário especificado
            var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            return dateTimeWithTimeZone;
        }
    }
}
