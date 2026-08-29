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
/// Extensões de <see cref="IServiceCollection"/> para bind de AppSettings
/// genéricos (Azure AD e Token JWT) a partir de <see cref="IConfiguration"/>,
/// registrando as instâncias como singletons no DI.
/// </summary>
public static class ServiceCollectionConfigureAppSettings
{
    /// <summary>
    /// Faz bind da seção AzureAd, registra <see cref="IAzureAdConfig"/> e devolve a instância.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configuração raiz da aplicação.</param>
    /// <returns>Instância configurada de <see cref="AzureAdConfig"/>.</returns>
    public static AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration)
    {
        var appValue = new AzureAdConfig();
        var configValue = ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
        new ConfigureFromConfigurationOptions<AzureAdConfig>(configValue).Configure(appValue);
        services.AddSingleton<IAzureAdConfig>(appValue);
        return appValue;
    }

    /// <summary>
    /// Faz bind da seção TokenConfigurations, registra as abstrações de token e devolve a instância.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configuração raiz da aplicação.</param>
    /// <returns>Instância configurada de <see cref="TokenConfigurationDto"/>.</returns>
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
