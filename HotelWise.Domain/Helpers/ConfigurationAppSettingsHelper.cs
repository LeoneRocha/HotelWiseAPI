using Microsoft.Extensions.Configuration;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class ConfigurationAppSettingsHelper
    {
        public static IConfiguration GetSectionApp(IConfiguration? configuration, string sectionName) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetSectionApp(configuration, sectionName);

        public static string GetConnectionStringApp(IConfiguration? configuration, string connectionName) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringApp(configuration, connectionName);

        public static string GetValueStringConfiguration(IConfiguration? configuration, string configurationName) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetValueStringConfiguration(configuration, configurationName);

        public static string GetConnectionStringMySQL(IConfiguration? configuration) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);

        public static IConfiguration GetRagConfig(IConfiguration configuration) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetRagConfig(configuration);

        public static IConfiguration GetTokenConfigurations(IConfiguration? configuration) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);

        public static IConfiguration GetAzureAdConfig(IConfiguration configuration) =>
            HotelWise.Core.SDK.Helpers.ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
    }
}
