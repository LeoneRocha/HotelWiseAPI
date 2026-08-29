namespace HotelWise.Domain.AppException
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.Exceptions.AppWarningException.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public class AppWarningException : HotelWise.Core.SDK.Common.Exceptions.AppWarningException
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
