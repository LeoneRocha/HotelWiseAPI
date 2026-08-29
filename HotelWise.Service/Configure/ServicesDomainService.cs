using System.Reflection;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Service.Entity;
using HotelWise.Service.Entity.HotelServices;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure;

/// <summary>
/// Configuração de injeção de dependência para os serviços de aplicação e entidades de domínio.
/// </summary>
public static class ServicesDomainService
{
    private const string ServiceSuffix = "Service";

    /// <summary>
    /// Registra manualmente serviços centrais de usuário e hotéis.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    public static void AddDependenciesManually(IServiceCollection services)
    {
        RegisterManuallyAddedServices(services);
    }

    /// <summary>
    /// Registra automaticamente os demais serviços por reflexão em lote.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    public static void AddDependenciesAuto(IServiceCollection services)
    {
        RegisterServices(services);
    }

    /// <summary>
    /// Registra os serviços adicionados explicitamente.
    /// </summary>
    private static void RegisterManuallyAddedServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHotelService, HotelService>();
    }

    /// <summary>
    /// Descobre e registra serviços nos assemblies carregados.
    /// </summary>
    private static void RegisterServices(IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.Load("HotelWise.Domain"),
            Assembly.Load("HotelWise.Data")
        };

        var ignoredInterfaces = new List<Type>
        {
            typeof(ITokenService),
            typeof(IHotelService),
            typeof(IUserService),
        };
        ignoredInterfaces.AddRange(ServiceCollectionHelper.GetRegisteredInterfaces(services));

        ServiceCollectionHelper.RegisterInterfaces(services, [ServiceSuffix], ignoredInterfaces, assemblies);
    }
}
