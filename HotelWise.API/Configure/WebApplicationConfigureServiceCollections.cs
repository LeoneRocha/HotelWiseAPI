using HotelWise.API.Configure;
using HotelWise.Service.Configure;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace HotelWise.API;

/// <summary>
/// Configuração central do container de injeção de dependências (IServiceCollection) da API, incluindo Telemetria, Swagger, MVC e formatadores de mídia.
/// </summary>
public static class WebApplicationConfigureServiceCollections
{
    /// <summary>
    /// Registra todos os serviços fundamentais do ASP.NET Core no container de DI.
    /// </summary>
    /// <param name="services">Coleção de serviços do container.</param>
    /// <param name="configuration">Configuração da aplicação (IConfiguration).</param>
    /// <param name="_logger">Logger raiz do Serilog.</param>
    public static void Configure(IServiceCollection services, IConfiguration configuration, Serilog.Core.Logger _logger)
    {
        ServiceCollectionConfigureCors.Configure(services);

        services.AddHealthChecks();

        var appInsightsConnectionString =
            configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
            ?? configuration["ApplicationInsights:ConnectionString"];

        if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
        {
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });
        }

        var appVersionInfo = LogAppHelper.GetInformationVersionProduct();
        var apiVersion = string.IsNullOrWhiteSpace(appVersionInfo.Version) || appVersionInfo.Version is "Unknown" or "Undefined"
            ? "v1"
            : appVersionInfo.Version;

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelWise.API", Version = apiVersion });
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
}

