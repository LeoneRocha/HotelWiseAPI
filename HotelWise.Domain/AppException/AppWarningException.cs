namespace HotelWise.Domain.AppException
{
    public class AppWarningException : Exception
    {
        public AppWarningException()
        {
        }
        public AppWarningException(string? message) : base(message)
        {
        }
        public AppWarningException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}