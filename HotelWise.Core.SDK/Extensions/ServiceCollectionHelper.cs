#if NET8_0_OR_GREATER
using System.Reflection;
using HotelWise.Core.SDK.Common;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Helpers de registro de serviços e descoberta de pares interface/implementação
/// por sufixo de classe em assemblies, para registro Scoped automático no DI.
/// </summary>
public static class ServiceCollectionHelper
{
    /// <summary>
    /// Remove de <paramref name="items"/> os elementos presentes em qualquer um dos filtros.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens.</typeparam>
    /// <param name="items">Array original.</param>
    /// <param name="filters">Arrays cujos elementos devem ser excluídos.</param>
    /// <returns>Array filtrado sem os itens presentes nos filtros.</returns>
    public static T[] FilterItems<T>(T[] items, params T[][] filters)
    {
        var filteredItems = items;
        foreach (var filter in filters)
        {
            filteredItems = filteredItems.Where(item => !filter.Contains(item)).ToArray();
        }
        return filteredItems;
    }

    /// <summary>
    /// Obtém o conjunto de interfaces já registradas como Scoped no container.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <returns>HashSet com os tipos de serviço Scoped registrados.</returns>
    public static HashSet<Type> GetRegisteredInterfaces(IServiceCollection services)
    {
        return services.Where(service => service.Lifetime == ServiceLifetime.Scoped)
                       .Select(service => service.ServiceType)
                       .ToHashSet();
    }

    /// <summary>
    /// Descobre classes concretas cujo nome termina com um dos sufixos e a interface
    /// correspondente <c>I{NomeDaClasse}</c>.
    /// </summary>
    /// <param name="classSuffixes">Sufixos de nome de classe (ex.: "Repository", "Service").</param>
    /// <param name="assemblies">Assemblies a varrer.</param>
    /// <returns>Array de <see cref="RepositoryInfo"/> com pares interface/implementação.</returns>
    public static RepositoryInfo[] GetInterfaces(string[] classSuffixes, params Assembly[] assemblies)
    {
        var repositories = assemblies.SelectMany(assembly => assembly.GetTypes())
                         .Where(type => type.IsClass && !type.IsAbstract && classSuffixes.Any(suffix => type.Name.EndsWith(suffix)))
                         .Select(type => new RepositoryInfo
                         {
                             InterfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}"),
                             ImplementationType = type
                         })
                         .Where(repo => repo.InterfaceType != null)
                         .ToArray();

        return repositories.ToArray();
    }

    /// <summary>
    /// Registra no DI (Scoped) as interfaces descobertas, ignorando as listadas em <paramref name="ignoredInterfaces"/>.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="classSuffixes">Sufixos de classe a considerar na descoberta.</param>
    /// <param name="ignoredInterfaces">Interfaces que não devem ser registradas.</param>
    /// <param name="assemblies">Assemblies a varrer.</param>
    public static void RegisterInterfaces(IServiceCollection services, string[] classSuffixes, List<Type> ignoredInterfaces, Assembly[] assemblies)
    {
        var interfaceInfos = GetInterfaces(classSuffixes, assemblies);
        interfaceInfos = interfaceInfos.OrderBy(i => i.InterfaceType!.Name).ToArray();

        var filteredInterfaces = FilterItems(interfaceInfos.Select(info => info.InterfaceType!).ToArray(), ignoredInterfaces.ToArray());
        filteredInterfaces = filteredInterfaces.OrderBy(i => i.Name).ToArray();

        foreach (var interfaceType in filteredInterfaces)
        {
            var implementationType = interfaceInfos.First(info => info.InterfaceType == interfaceType).ImplementationType;
            if (implementationType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
#endif
