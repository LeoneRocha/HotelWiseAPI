#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Leitura de seções e connection strings de <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationAppSettingsHelper
{
    public static IConfiguration GetSectionApp(IConfiguration? configuration, string sectionName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        return configuration.GetSection(sectionName);
    }

    public static string GetConnectionStringApp(IConfiguration? configuration, string connectionName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        return configuration.GetConnectionString(connectionName) ?? string.Empty;
    }

    public static string GetValueStringConfiguration(IConfiguration? configuration, string configurationName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        string appsettingsValue = configuration[configurationName] ?? string.Empty;
        return appsettingsValue;
    }

    public static string GetConnectionStringMySQL(IConfiguration? configuration)
    {
        return GetConnectionStringApp(configuration, "DBConnectionMySQL");
    }

    public static IConfiguration GetRagConfig(IConfiguration configuration)
    {
        return GetSectionApp(configuration, "Rag");
    }

    public static IConfiguration GetTokenConfigurations(IConfiguration? configuration)
    {
        return GetSectionApp(configuration, "TokenConfigurations");
    }

    public static IConfiguration GetAzureAdConfig(IConfiguration configuration)
    {
        return GetSectionApp(configuration, "AzureAd");
    }
}
#endif
