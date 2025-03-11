using HotelWise.Data.Context;
using HotelWise.Domain.Helpers;
using HotelWise.Service.Configure;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HotelWise.API.Configure
{
    public static class ServiceCollectionAddAllDependencies
    {
        public static void Configure(IServiceCollection services, Serilog.Core.Logger _logger, IConfiguration configuration)
        {
            services.AddSingleton<Serilog.ILogger>(sp =>
            {
                return _logger;
            }); 
            addORM(services, configuration);

            ServiceCollectionConfigureServicesDomain.Configure(services, configuration);
        }

        private static void addORM(IServiceCollection services, IConfiguration configuration)
        {
            var connection = ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);

            services.AddPooledDbContextFactory<HotelWiseDbContextMysql>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 21))));


            services.AddDbContext<HotelWiseDbContextMysql>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection),
                optionsMySQL =>
                {
                    optionsMySQL.MigrationsAssembly("HotelWise.Data");
                    optionsMySQL.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                });
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);
        }
    }
}
