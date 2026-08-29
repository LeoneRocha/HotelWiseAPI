using HotelWise.Core.SDK.Helpers;
using HotelWise.Data.Context;
using HotelWise.Service.Configure;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HotelWise.API.Configure;

/// <summary>
/// Configuração de injeção de dependências para o Entity Framework Core (MySQL), Serilog e serviços de domínio.
/// </summary>
public static class ServiceCollectionAddAllDependencies
{
    /// <summary>
    /// Registra o logger estruturado, a fábrica de contextos de banco de dados e os serviços de domínio.
    /// </summary>
    /// <param name="services">Coleção de serviços do container DI.</param>
    /// <param name="_logger">Instância principal do Serilog Logger.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    public static void Configure(IServiceCollection services, Serilog.Core.Logger _logger, IConfiguration configuration)
    {
        services.AddSingleton<Serilog.ILogger>(sp =>
        {
            return _logger;
        });
        addORM(services, configuration);

        ServiceCollectionConfigureServicesDomain.Configure(services, configuration);
    }

    /// <summary>
    /// Configura o pool de DbContext e a conexão com o MySQL através do Pomelo EF Core.
    /// </summary>
    private static void addORM(IServiceCollection services, IConfiguration configuration)
    {
        var connection = ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);
        if (string.IsNullOrWhiteSpace(connection))
        {
            throw new InvalidOperationException(
                "Connection string MySQL ausente: 'ConnectionStrings:DBConnectionMySQL'. " +
                "Defina no appsettings do ambiente de publicação ou na variável 'ConnectionStrings__DBConnectionMySQL'. " +
                "Sem isso a API falha no startup (HTTP 500.30 no IIS).");
        }

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

        services.AddPooledDbContextFactory<HotelWiseDbContextMysql>(options =>
            options.UseMySql(connection, serverVersion));

        services.AddDbContext<HotelWiseDbContextMysql>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.UseMySql(connection, serverVersion,
            optionsMySQL =>
            {
                optionsMySQL.MigrationsAssembly("HotelWise.Data");
                optionsMySQL.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            });
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);
    }
}
