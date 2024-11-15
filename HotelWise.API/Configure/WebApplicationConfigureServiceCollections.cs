using HotelWise.Data.Context;
using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Service.Configure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Filters;

namespace HotelWise.API
{
    public static class WebApplicationConfigureServiceCollections
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, Serilog.Core.Logger _logger)
        {
#pragma warning disable S5122 // Disabling Sonar warning for CORS
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition");
            }));
#pragma warning restore S5122


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelWise.API", Version = "v1" });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddEndpointsApiExplorer();

            //AcceptHeader 
            services.AddControllers();
            //AddMvc
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization()
                .AddXmlSerializerFormatters();



            services.AddLogging();
            services.AddSingleton<Serilog.ILogger>(sp =>
            {
                return _logger;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
             
            ConfigureServicesAI.ConfigureServices(services);
             
            addORM(services, configuration);

            #region KERNEL  
            SemanticKernelProviderConfigure.SetupSemanticKernelProvider(services, configuration);
            #endregion KERNEL
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