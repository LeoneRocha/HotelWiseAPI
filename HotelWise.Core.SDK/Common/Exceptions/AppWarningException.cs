namespace HotelWise.Core.SDK.Common.Exceptions;

/// <summary>
/// Exceção de aviso da aplicação, indicando uma condição não fatal
/// (por exemplo, validação de negócio ou alerta operacional) que deve ser
/// tratada e comunicada sem interromper o fluxo como erro crítico.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Exceptions.AppWarningException. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AppWarningException : SmartCoreHub.Core.SDK.Common.Exceptions.AppWarningException
{
    public AppWarningException()
    {
    }

    public AppWarningException(string? message)
        : base(message)
    {
    }

    public AppWarningException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
