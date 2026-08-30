using System.Reflection;
using HotelWise.Data.Repository;
using HotelWise.Data.Repository.HotelRepositories;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure;

/// <summary>
/// Configuração de injeção de dependência para os repositórios de dados do domínio.
/// </summary>
public static class ServicesDomainRepository
{
    private const string RepositorySuffix = "Repository";

    /// <summary>
    /// Registra manualmente as dependências principais de repositórios escopados.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    public static void AddDependenciesManually(IServiceCollection services)
    {
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    /// <summary>
    /// Registra automaticamente os demais repositórios escaneando os assemblies de Domínio e Dados.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    public static void AddDependenciesAuto(IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.Load("HotelWise.Domain"),
            Assembly.Load("HotelWise.Data")
        };

        var ignoredInterfaces = new List<Type>
        {
            typeof(IHotelRepository),
            typeof(IUserRepository),
        };

        ServiceCollectionHelper.RegisterInterfaces(services, [RepositorySuffix], ignoredInterfaces, assemblies);
    }
}

