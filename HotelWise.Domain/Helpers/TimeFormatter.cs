namespace HotelWise.Domain.Helpers
{
    public static class TimeFormatter
    {
        public static string FormatElapsedTime(TimeSpan elapsed)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
    }
}
