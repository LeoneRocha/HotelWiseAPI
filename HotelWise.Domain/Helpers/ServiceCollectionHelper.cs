using HotelWise.Domain.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.ServiceCollectionHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class ServiceCollectionHelper
    {
        public static T[] FilterItems<T>(T[] items, params T[][] filters) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionHelper.FilterItems(items, filters);

        public static HashSet<Type> GetRegisteredInterfaces(IServiceCollection services) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionHelper.GetRegisteredInterfaces(services);

        public static RepositoryInfo[] GetInterfaces(string[] classSuffixes, params Assembly[] assemblies) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionHelper.GetInterfaces(classSuffixes, assemblies)
                .Select(r => new RepositoryInfo
                {
                    InterfaceType = r.InterfaceType,
                    ImplementationType = r.ImplementationType
                }).ToArray();

        public static void RegisterInterfaces(IServiceCollection services, string[] classSuffixes, List<Type> ignoredInterfaces, Assembly[] assemblies) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionHelper.RegisterInterfaces(services, classSuffixes, ignoredInterfaces, assemblies);
    }
}
