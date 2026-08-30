using FluentValidation;
using HotelWise.Domain.Validator.HotelValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelWise.Service.Configure;

/// <summary>
/// Registrador centralizado de dependências de domínio, repositórios, serviços, AutoMapper, validadores e pipeline de IA.
/// </summary>
public static class ServiceCollectionConfigureServicesDomain
{
    /// <summary>
    /// Configura todos os serviços, repositórios, validadores e provedores de IA da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="_configuration">Instância de configuração global (IConfiguration).</param>
    public static void Configure(IServiceCollection services, IConfiguration _configuration)
    {
        // AutoMapper
        ServiceCollectionConfigureAutoMapper.Configure(services);
        AddDependenciesSingleton(services);

        ServicesDomainRepository.AddDependenciesManually(services);

        ServicesDomainService.AddDependenciesManually(services);

        ConfigureServicesAI.ConfigureServices(services);

        #region KERNEL  
        SemanticKernelProviderConfigure.SetupSemanticKernelProvider(services, _configuration);
        #endregion KERNEL

        // Validators
        services.AddValidatorsFromAssemblyContaining<HotelValidator>();

        ServicesDomainRepository.AddDependenciesAuto(services);
        ServicesDomainService.AddDependenciesAuto(services);
    }

    /// <summary>
    /// Registra dependências singleton utilitárias como HttpContextAccessor e TokenService.
    /// </summary>
    private static void AddDependenciesSingleton(IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddSingleton<ITokenService, TokenService>();
    }
}

