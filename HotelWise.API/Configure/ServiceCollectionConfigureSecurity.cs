using HotelWise.Core.SDK.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelWise.API.Configure
{
    public static class ServiceCollectionConfigureSecurity
    {
        private const string AzureAdSchemeName = "AzureAd";

        public static void Configure(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
        {
            addSecurity(services, tokenConfigurations, configuration);
        }

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
}
