using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure;

/// <summary>
/// Provedor de configuração e inicialização do Semantic Kernel para o tipo vetorial <see cref="HotelVector"/>.
/// </summary>
public static class SemanticKernelProviderConfigure
{
    /// <summary>
    /// Registra o provedor Semantic Kernel configurado com os serviços de chat e embeddings definidos.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configurações da aplicação (IConfiguration).</param>
    public static void SetupSemanticKernelProvider(IServiceCollection services, IConfiguration configuration)
    {
        HotelWise.Core.SDK.AI.Configure.SemanticKernelProviderConfigure
            .SetupSemanticKernelProvider<HotelVector>(services, configuration);
    }
}

/// <summary>
/// Configuração de injeção de dependência para o AutoMapper escaneando perfis do domínio.
/// </summary>
public static class ServiceCollectionConfigureAutoMapper
{
    /// <summary>
    /// Registra os perfis de mapeamento do AutoMapper presentes no assembly de domínio.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void Configure(IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(DomainMappingProfile));
    }
}

/// <summary>
/// Configuração de injeção de dependência para leitura de opções e configurações do AppSettings (Azure AD e Token JWT).
/// </summary>
public static class ServiceCollectionConfigureAppSettings
{
    /// <summary>
    /// Faz o bind e registra a configuração do Azure AD / Entra ID.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>Instância de <see cref="AzureAdConfig"/> preenchida.</returns>
    public static AzureAdConfig AddAndReturnAzureAdConfig(IServiceCollection services, IConfiguration configuration) =>
        HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAppSettings.AddAndReturnAzureAdConfig(services, configuration);

    /// <summary>
    /// Faz o bind e registra as opções de emissão de token JWT.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>Instância de <see cref="TokenConfigurationDto"/> preenchida.</returns>
    public static TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration configuration) =>
        HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAppSettings.AddAndReturnTokenConfiguration(services, configuration);
}

/// <summary>
/// Configuração de políticas de CORS para permissão de origens nos clientes frontend.
/// </summary>
public static class ServiceCollectionConfigureCors
{
    /// <summary>
    /// Aplica as políticas CORS padrão configuradas pelo SDK.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void Configure(IServiceCollection services) =>
        HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureCors.Configure(services);
}
