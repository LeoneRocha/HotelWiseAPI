#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Leitura tipada de seções e connection strings a partir de <see cref="IConfiguration"/>.
/// Centraliza nomes de seções conhecidos (Rag, TokenConfigurations, AzureAd, MySQL)
/// e valida presença da configuração antes do acesso.
/// </summary>
/// <example>
/// <code>
/// var tokenSection = ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);
/// var mysql = ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ConfigurationAppSettingsHelper
{
    public static IConfiguration GetSectionApp(IConfiguration? configuration, string sectionName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetSectionApp(configuration, sectionName);

    public static string GetConnectionStringApp(IConfiguration? configuration, string connectionName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringApp(configuration, connectionName);

    public static string GetValueStringConfiguration(IConfiguration? configuration, string configurationName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetValueStringConfiguration(configuration, configurationName);

    public static string GetConnectionStringMySQL(IConfiguration? configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);

    public static IConfiguration GetRagConfig(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetRagConfig(configuration);

    public static IConfiguration GetTokenConfigurations(IConfiguration? configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);

    public static IConfiguration GetAzureAdConfig(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
}

#endif
