#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Helpers de registro de serviços e descoberta de pares interface/implementação
/// por sufixo de classe em assemblies, para registro Scoped automático no DI.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionHelper
{
    public static T[] FilterItems<T>(T[] items, params T[][] filters) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.FilterItems<T>(items, filters);

    public static HashSet<Type> GetRegisteredInterfaces(IServiceCollection services) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.GetRegisteredInterfaces(services);

    public static SmartCoreHub.Core.SDK.Common.RepositoryInfo[] GetInterfaces(string[] classSuffixes, params Assembly[] assemblies) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.GetInterfaces(classSuffixes, assemblies);

    public static void RegisterInterfaces(IServiceCollection services, string[] classSuffixes, List<Type> ignoredInterfaces, Assembly[] assemblies) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper.RegisterInterfaces(services, classSuffixes, ignoredInterfaces, assemblies);
}

#endif
