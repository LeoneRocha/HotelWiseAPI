﻿using HotelWise.API.Configure;
using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Service.Configure;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HotelWise.API
{
    public static class WebApplicationConfigureServiceCollections
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, Serilog.Core.Logger _logger)
        {

            configureCors(services);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelWise.API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
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

            ServiceCollectionAddAllDependencies.Configure(services, _logger, configuration);

            //Security API
            var tokenConfigurations = ServiceCollectionConfigureAppSettings.AddAndReturnTokenConfiguration(services, configuration);
            var azureConfig = ServiceCollectionConfigureAppSettings.AddAndReturnAzureAdConfig(services, configuration);            
            ServiceCollectionConfigureSecurity.Configure(services, tokenConfigurations, configuration, azureConfig);
        }

        private static void configureCors(IServiceCollection services)
        {
#pragma warning disable S5122 // Disabling Sonar warning for CORS
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition");
            }));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
#pragma warning restore S5122
        } 
    }
}