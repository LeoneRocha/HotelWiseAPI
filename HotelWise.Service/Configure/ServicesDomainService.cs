using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.AppConfig;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelWise.Service.Configure
{
    public static class ServicesDomainService
    {
        private const string ServiceSuffix = "Service";

        public static void AddDependenciesManually(IServiceCollection services)
        {
            RegisterManuallyAddedServices(services); 
        }
        public static void AddDependenciesAuto(IServiceCollection services)
        {
            RegisterServices(services);
        }
        private static void RegisterManuallyAddedServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHotelService, HotelService>();
        }
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
}
