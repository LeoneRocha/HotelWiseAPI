using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    /// <summary>
    /// Configura Semantic Kernel / RAG via Core para o tipo hotel <see cref="HotelVector"/>.
    /// </summary>
    public static class SemanticKernelProviderConfigure
    {
        public static void SetupSemanticKernelProvider(IServiceCollection services, IConfiguration configuration)
        {
            HotelWise.Core.SDK.AI.Configure.SemanticKernelProviderConfigure
                .SetupSemanticKernelProvider<HotelVector>(services, configuration);
        }
    }

    /// <summary>
    /// Registra AutoMapper escaneando o assembly Domain (profiles em Mapper/).
    /// </summary>
    public static class ServiceCollectionConfigureAutoMapper
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(_ => { }, typeof(DomainMappingProfile));
        }
    }

    /// <summary>
    /// Bind de AppSettings (Azure AD / Token) via Core.
    /// </summary>
    public static class ServiceCollectionConfigureAppSettings
    {
        public static AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAppSettings.AddAndReturnAzureAdConfig(services, configuration);

        public static TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration configuration) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAppSettings.AddAndReturnTokenConfiguration(services, configuration);
    }

    /// <summary>
    /// CORS genérico via Core.
    /// </summary>
    public static class ServiceCollectionConfigureCors
    {
        public static void Configure(IServiceCollection services) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureCors.Configure(services);
    }
}
