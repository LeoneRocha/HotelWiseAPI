
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração geral da aplicação.
/// Agrupa content-types HTTP, formatos de data/hora e mensagens de configuração inválida.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants em SmartCoreHub.Core.SDK.")]
public static class AppConfigConstants
{
    /// <summary>Content-type JSON para requisições e respostas HTTP.</summary>
    public const string ApplicationContentJon =
        SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants.ApplicationContentJson;

    /// <summary>Formato padrão de data e hora para exibição e logs.</summary>
    public const string DATE_FORMAT =
        SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants.DateFormat;

    /// <summary>Formato ISO-8601 UTC de data e hora.</summary>
    public const string DATE_FORMAT2 =
        SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants.DateFormatIso;

    /// <summary>Mensagem padrão quando a configuração da aplicação é nula.</summary>
    public const string ConfigurationConfigurationNotBeNull =
        SmartCoreHub.Core.SDK.Service.Configuration.AppConfigConstants.ConfigurationCannotBeNull;
}
