#if NET8_0_OR_GREATER
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchConfig = SmartCoreHub.Core.SDK.Domain.AI.Configuration;
using SchDi = SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions;
using SchSecurity = SmartCoreHub.Core.SDK.Common.Security;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// DI bind de AppSettings — delega ao SCH.
/// </summary>
public static class ServiceCollectionConfigureAppSettings
{
    /// <summary>
    /// Registra e retorna as configurações do Azure AD / Microsoft Entra ID.
    /// </summary>
    public static SchConfig.AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration) =>
        SchDi.ServiceCollectionConfigureAppSettings.AddAndReturnAzureAdConfig(services, configuration);

    /// <summary>
    /// Registra e retorna as configurações de token JWT.
    /// </summary>
    public static SchSecurity.TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration configuration) =>
        SchDi.ServiceCollectionConfigureAppSettings.AddAndReturnTokenConfiguration(services, configuration);
}
#endif
