#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Helpers de registro de serviços e descoberta de pares interface/implementação
/// por sufixo de classe em assemblies, para registro Scoped automático no DI.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper em SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionHelper
{
    /// <summary>
    /// Filtra itens de um array aplicando listas de exclusão.
    /// </summary>
    /// <typeparam name="T">Tipo dos elementos.</typeparam>
    /// <param name="items">Itens originais.</param>
    /// <param name="filters">Filtros de exclusão.</param>
    /// <returns>Array filtrado.</returns>
    public static T[] FilterItems<T>(T[] items, params T[][] filters) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.FilterItems<T>(items, filters);

    /// <summary>
    /// Obtém os tipos de interfaces já registradas no container.
    /// </summary>
    /// <param name="services">Coleção de serviços do DI.</param>
    /// <returns>Conjunto de interfaces registradas.</returns>
    public static HashSet<Type> GetRegisteredInterfaces(IServiceCollection services) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.GetRegisteredInterfaces(services);

    /// <summary>
    /// Obtém os pares de interface e implementação por sufixos nos assemblies informados.
    /// </summary>
    /// <param name="classSuffixes">Sufixos de classe a buscar.</param>
    /// <param name="assemblies">Assemblies para escaneamento.</param>
    /// <returns>Array de pares de repositório/serviço encontrados.</returns>
    public static SmartCoreHub.Core.SDK.Common.RepositoryInfo[] GetInterfaces(string[] classSuffixes, params Assembly[] assemblies) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.GetInterfaces(classSuffixes, assemblies);

    /// <summary>
    /// Registra dinamicamente pares interface/implementação no container como Scoped.
    /// </summary>
    /// <param name="services">Coleção de serviços do DI.</param>
    /// <param name="classSuffixes">Sufixos de classe a buscar.</param>
    /// <param name="ignoredInterfaces">Interfaces a serem ignoradas no registro.</param>
    /// <param name="assemblies">Assemblies para escaneamento.</param>
    public static void RegisterInterfaces(IServiceCollection services, string[] classSuffixes, List<Type> ignoredInterfaces, Assembly[] assemblies) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.RegisterInterfaces(services, classSuffixes, ignoredInterfaces, assemblies);
}
#endif
