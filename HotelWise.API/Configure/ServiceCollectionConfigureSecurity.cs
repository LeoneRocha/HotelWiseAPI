using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

namespace HotelWise.API.Configure;

/// <summary>
/// Configuração do subsistema de segurança da API, definindo esquemas de autenticação JWT Bearer e integração com Azure AD / Entra ID.
/// </summary>
public static class ServiceCollectionConfigureSecurity
{
    private const string AzureAdSchemeName = "AzureAd";

    /// <summary>
    /// Registra e configura a autenticação JWT simétrica e a integração com Microsoft Identity Web.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="tokenConfigurations">Parâmetros de configuração do token JWT interno.</param>
    /// <param name="configuration">Configurações globais da aplicação.</param>
    /// <param name="azureConfig">Configurações de integração com Azure AD.</param>
    public static void Configure(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
    {
        addSecurity(services, tokenConfigurations, configuration);
    }

    /// <summary>
    /// Configura esquemas de autenticação e políticas de autorização Bearer e AzureAd.
    /// </summary>
    private static void addSecurity(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = tokenConfigurations.Issuer,
                ValidAudience = tokenConfigurations.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                    logger.Error("JWT Authentication failed.", context.Exception);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                    logger.Information("JWT Token validated successfully.");
                    return Task.CompletedTask;
                }
            };
        })
        .AddMicrosoftIdentityWebApi(options =>
        {
            configuration.Bind(AzureAdSchemeName, options);
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                    logger.Error("AzureAD Authentication failed.", context.Exception);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                    logger.Information("AzureAD Token validated successfully.");
                    return Task.CompletedTask;
                }
            };
        }, options =>
        {
            configuration.Bind(AzureAdSchemeName, options);
            options.TokenValidationParameters.ValidAudiences = new[] { tokenConfigurations.Audience };
        }, AzureAdSchemeName);

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(AzureAdSchemeName, policy =>
            {
                policy.AddAuthenticationSchemes(AzureAdSchemeName);
                policy.AuthenticationSchemes.Add(AzureAdSchemeName);
                policy.RequireAuthenticatedUser();
            });
        });
    }
}

