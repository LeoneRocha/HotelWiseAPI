#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Bind de AppSettings genéricos (Azure AD / Token).
/// </summary>
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
#endif
