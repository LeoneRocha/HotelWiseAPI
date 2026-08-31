namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração geral da aplicação.
/// Agrupa content-types HTTP, formatos de data/hora e mensagens de configuração inválida.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.AppConfigConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class AppConfigConstants
{
    /// <summary>Content-type JSON para requisições e respostas HTTP.</summary>
    public const string ApplicationContentJon = "application/json";

    /// <summary>Formato padrão de data e hora para exibição e logs.</summary>
    public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>Formato ISO-8601 UTC de data e hora.</summary>
    public const string DATE_FORMAT2 = "yyyy-MM-ddTHH:mm:ssZ";

    /// <summary>Mensagem padrão quando a configuração da aplicação é nula.</summary>
    public const string ConfigurationConfigurationNotBeNull = "Configuration cannot be null.";
}
