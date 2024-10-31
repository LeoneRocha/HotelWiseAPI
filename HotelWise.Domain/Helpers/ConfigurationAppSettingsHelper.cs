using HotelWise.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Domain.Helpers
{
    public static class ConfigurationAppSettingsHelper
    {
        #region GENERIC
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


        #endregion GENERIC

        public static IConfiguration GetCacheConfiguration(IConfiguration? configuration)
        {
            return GetSectionApp(configuration, "CacheConfiguration");
        }
        public static IConfiguration GetAuthConfiguration(IConfiguration? configuration)
        {
            return GetSectionApp(configuration, "AuthConfiguration");
        }

        public static IConfiguration GetTokenConfigurations(IConfiguration? configuration)
        {
            return GetSectionApp(configuration, "TokenConfigurations");
        }

        public static string GetConnectionStringMySQL(IConfiguration? configuration)
        {
            return GetConnectionStringApp(configuration, "SmartDigitalPsicoDBConnectionMySQL");
        }

        public static string GetConnectionStringSQL(IConfiguration? configuration)
        {
            return GetConnectionStringApp(configuration, "SmartDigitalPsicoDBConnectionSQLServer");
        }

        public static IConfiguration GetDataBaseConfigurations(IConfiguration? configuration)
        {
            return GetSectionApp(configuration, "DataBaseConfigurations");
        }

        public static string GetAppSettingsResourcesTemp(IConfiguration? configuration)
        {
            return GetValueStringConfiguration(configuration, "AppSettings:ResourcesTemp");
        }

        public static IConfiguration GetIResiliencePolicyConfig(IConfiguration configuration)
        {
            return GetSectionApp(configuration, "ResiliencePolicyConfig");
        }

        public static IConfiguration GetLocationSaveFileConfigurationVO(IConfiguration configuration)
        {
            return GetSectionApp(configuration, "LocationSaveFileConfigurationVO");
        }
        public static IConfiguration GetSmtpSettings(IConfiguration configuration)
        {
            return GetSectionApp(configuration, "SmtpSettings");
        }

        public static string[] GetAllowedFileExtensions(IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:AllowedFileExtensions").Get<string[]>() ?? [];
        }
        public static string[] GetAllowedContentTypes(IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:AllowedContentTypes").Get<string[]>() ?? [];
        }
        public static long GetMaxFileSizeMegabytes(IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:MaxFileSizeMegabytes").Get<long>();
        }

        public static string GetStorageServicesAzureStorageConnectionString(IConfiguration configuration)
        {
            return configuration.GetSection("StorageServices:AzureStorage")["ConnectionString"] ?? string.Empty;
        }

        public static string GetStorageServicesAzureStorageDaysExpiresBlobSas(IConfiguration configuration)
        {
            return configuration.GetSection("StorageServices:AzureStorage")["DaysExpiresBlobSas"] ?? string.Empty;
        }

        public static string GetSecuritySettingsAesSettingAesKey(IConfiguration configuration)
        {
            return configuration.GetSection("SecuritySettings:AesSettings")["AesKey"] ?? string.Empty;
        }
        public static string GetSecuritySettingsAesSettingAesIv(IConfiguration configuration)
        {
            return configuration.GetSection("SecuritySettings:AesSettings")["AesIv"] ?? string.Empty;
        }
    }
}
