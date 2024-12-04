using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelWise.API.Configure
{
    public static class ServiceCollectionConfigureSecurity
    {
        public static void Configure(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration)
        {
            //addSecuritySimple(services, tokenConfigurations, configuration);
            addSecurity(services, tokenConfigurations, configuration);
        }

        private static void addSecuritySimple(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration)
        {
            var adSec = configuration.GetSection("AzureAd");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(adSec); //FUNCIONA SO COM ISSO 
        }

        private static void addSecurity(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration)
        {
            var adScheme = AzureADEntraIDConstants.AzureAd;
            var adSec = configuration.GetSection("AzureAd");

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
                configuration.Bind("AzureAd", options);
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
                configuration.Bind("AzureAd", options);
                options.TokenValidationParameters.ValidAudiences = new[] { tokenConfigurations.Audience };
            }, "AzureAd");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("AzureAd", policy =>
                {
                    policy.AddAuthenticationSchemes("AzureAd");
                    policy.AuthenticationSchemes.Add("AzureAd");
                    policy.RequireAuthenticatedUser();
                });
            });
        }

    }
}
