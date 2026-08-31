namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração geral da aplicação.
/// Agrupa content-types HTTP, formatos de data/hora e mensagens de configuração inválida.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.AppConfigConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class AppConfigConstants
{
    public const string ApplicationContentJon = "application/json";

    public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public const string DATE_FORMAT2 = "yyyy-MM-ddTHH:mm:ssZ";

    public const string ConfigurationConfigurationNotBeNull = "Configuration cannot be null.";
}
