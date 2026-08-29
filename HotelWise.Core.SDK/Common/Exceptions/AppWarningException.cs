namespace HotelWise.Core.SDK.Common.Exceptions;

/// <summary>
/// Exceção de aviso de aplicação (não fatal).
/// </summary>
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
