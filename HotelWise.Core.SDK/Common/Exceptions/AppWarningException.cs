namespace HotelWise.Core.SDK.Common.Exceptions;

/// <summary>
/// Exceção de aviso da aplicação, indicando uma condição não fatal
/// (por exemplo, validação de negócio ou alerta operacional) que deve ser
/// tratada e comunicada sem interromper o fluxo como erro crítico.
/// </summary>
public class AppWarningException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppWarningException"/> sem mensagem.
    /// </summary>
    public AppWarningException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppWarningException"/> com a mensagem especificada.
    /// </summary>
    /// <param name="message">Mensagem que descreve o aviso; pode ser <c>null</c>.</param>
    public AppWarningException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppWarningException"/> com mensagem e exceção interna.
    /// </summary>
    /// <param name="message">Mensagem que descreve o aviso; pode ser <c>null</c>.</param>
    /// <param name="innerException">Exceção que causou o aviso atual; pode ser <c>null</c>.</param>
    public AppWarningException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
