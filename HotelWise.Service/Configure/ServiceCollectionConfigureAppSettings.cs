using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Dto.AppConfig.Rag;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.AppConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelWise.Service.Configure
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — host retorna subclasses Domain (herdam Core).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAppSettings.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DI")]
    public static class ServiceCollectionConfigureAppSettings
    {
        public static AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration)
        {
            var appValue = new AzureAdConfig();
            var configValue = ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
            new ConfigureFromConfigurationOptions<AzureAdConfig>(configValue).Configure(appValue);
            services.AddSingleton<IAzureAdConfig>(appValue);
            return appValue;
        }

        public static TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var configValue = ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);
            var tokenConfigurations = new TokenConfigurationDto();
            new ConfigureFromConfigurationOptions<TokenConfigurationDto>(configValue).Configure(tokenConfigurations);
            services.AddSingleton<ITokenConfigurationDto>(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            return tokenConfigurations;
        }
    }
}
