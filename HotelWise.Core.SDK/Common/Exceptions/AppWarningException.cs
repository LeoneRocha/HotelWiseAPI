using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common.Exceptions;

/// <summary>
/// Exceção de aviso da aplicação, indicando uma condição não fatal
/// (por exemplo, validação de negócio ou alerta operacional) que deve ser
/// tratada e comunicada sem interromper o fluxo como erro crítico.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.Exceptions.AppWarningException", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.Exceptions.AppWarningException em SmartCoreHub.Core.SDK.")]
public class AppWarningException : SmartCoreHub.Core.SDK.Common.Exceptions.AppWarningException
{
    /// <summary>Inicializa uma nova instância de <see cref="AppWarningException"/>.</summary>
    public AppWarningException()
    {
    }

    /// <summary>Inicializa uma nova instância de <see cref="AppWarningException"/> com mensagem descritiva.</summary>
    /// <param name="message">Mensagem de aviso.</param>
    public AppWarningException(string? message)
        : base(message)
    {
    }

    /// <summary>Inicializa uma nova instância de <see cref="AppWarningException"/> com mensagem e exceção interna.</summary>
    /// <param name="message">Mensagem de aviso.</param>
    /// <param name="innerException">Exceção interna que causou o aviso.</param>
    public AppWarningException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
