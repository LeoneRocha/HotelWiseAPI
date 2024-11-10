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
        public static string GetConnectionStringMySQL(IConfiguration? configuration)
        {
            return GetConnectionStringApp(configuration, "DBConnectionMySQL");
        }

        public static IConfiguration GetVectorStoreSettingsDto(IConfiguration configuration)
        {
            return GetSectionApp(configuration, "VectorStoreSettings");
        }
    }
}
