#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// DI bind de AppSettings — registra tipos HW (casca) no container.
/// Lógica de bind espelha SCH; tipos retornados permanecem HW para hosts.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAppSettings. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureAppSettings
{
    public static AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration)
    {
        var appValue = new AzureAdConfig();
        var configValue = Helpers.ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
        new ConfigureFromConfigurationOptions<AzureAdConfig>(configValue).Configure(appValue);
        services.AddSingleton<IAzureAdConfig>(appValue);
        return appValue;
    }

    public static TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var configValue = Helpers.ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);
        var tokenConfigurations = new TokenConfigurationDto();
        new ConfigureFromConfigurationOptions<TokenConfigurationDto>(configValue).Configure(tokenConfigurations);
        services.AddSingleton<ITokenConfigurationDto>(tokenConfigurations);
        services.AddSingleton(tokenConfigurations);
        return tokenConfigurations;
    }
}
#endif
