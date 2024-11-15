﻿using HotelWise.Data.Context;
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

            addRagConfig(services, configuration);

            var appConfig = addApplicationConfig(services, configuration);
            addORM(services, configuration);

            #region KERNEL 

            services.AddKernel();

            var builder = Kernel.CreateBuilder();

            // Register the kernel with the dependency injection container
            // and add Chat Completion and Text Embedding Generation services.
            var kernelBuilder =
#pragma warning disable SKEXP0020
            builder.AddQdrantVectorStoreRecordCollection<ulong, HotelVector>(appConfig.RagConfig.CollectionName, appConfig.QdrantConfig.Host, appConfig.QdrantConfig.Port, appConfig.QdrantConfig.Https, appConfig.QdrantConfig.ApiKey);

            builder.AddQdrantVectorStore(appConfig.QdrantConfig.Host, appConfig.QdrantConfig.Port, appConfig.QdrantConfig.Https, appConfig.QdrantConfig.ApiKey, options: new QdrantVectorStoreOptions { HasNamedVectors = true });

            var kernel = builder.Build();
            IVectorStore vectorStore = kernel.GetRequiredService<IVectorStore>();
            services.AddSingleton(vectorStore);
            #endregion KERNEL
#pragma warning restore SKEXP0020
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

        //private static void addVectorStoreConfig(IServiceCollection services, IConfiguration configuration)
        //{
        //    // Bind the PolicyConfig section of appsettings.json to the PolicyConfig class
        //    var appSettingsValue = new VectorStoreSettingsDto();

        //    var configValue = ConfigurationAppSettingsHelper.GetVectorStoreSettingsDto(configuration);

        //    new ConfigureFromConfigurationOptions<VectorStoreSettingsDto>(configValue).Configure(appSettingsValue);
        //    // Register the PolicyConfig instance as a singleton
        //    services.AddSingleton<IVectorStoreSettingsDto>(appSettingsValue);
        //}

        private static void addRagConfig(IServiceCollection services, IConfiguration configuration)
        {
            // Bind the PolicyConfig section of appsettings.json to the PolicyConfig class
            var appSettingsValue = new RagConfig();

            var configValue = ConfigurationAppSettingsHelper.GetVectorStoreSettingsDto(configuration);

            new ConfigureFromConfigurationOptions<RagConfig>(configValue).Configure(appSettingsValue);
            // Register the PolicyConfig instance as a singleton
            services.AddSingleton<IRagConfig>(appSettingsValue);
        }

        private static ApplicationConfig addApplicationConfig(IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = new ApplicationConfig(configuration);

            // Register the PolicyConfig instance as a singleton
            services.AddSingleton<IApplicationConfig>(appConfig);
            return appConfig;
        }
    }
}