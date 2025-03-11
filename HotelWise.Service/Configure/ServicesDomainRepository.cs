using HotelWise.Data.Repository;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelWise.Service.Configure
{
    public static class ServicesDomainRepository
    {
        private const string RepositorySuffix = "Repository";

        public static void AddDependencies(IServiceCollection services)
        {
            RegisterManuallyAddedServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterManuallyAddedServices(IServiceCollection services)
        {
             // services.AddScoped<IHotelRepository, HotelRepository>(); 
        }

        private static void RegisterRepositories(IServiceCollection services)
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
}
