using HotelWise.Data.Repository;
using HotelWise.Data.Repository.HotelRepositories;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces;
using HotelWise.Domain.Interfaces.Entity.IA;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelWise.Service.Configure
{
    public static class ServicesDomainRepository
    {
        private const string RepositorySuffix = "Repository";

        public static void AddDependenciesManually(IServiceCollection services)
        {
            services.AddScoped<IHotelRepository, HotelRepository>(); 
            services.AddScoped<IUserRepository, UserRepository>();
        }

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
}
