using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.AppConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelWise.API.Configure
{
    public static class ServiceCollectionConfigureSecurity
    {
        public static void Configure(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
        {
            addSecurity(services, tokenConfigurations, configuration);
            //addSecuritySimple(services, tokenConfigurations, configuration);
            ///addSecuritySimpleJWT1(services, tokenConfigurations, configuration, azureConfig);
            //https://learn.microsoft.com/en-us/samples/azure-samples/ms-identity-ciam-javascript-tutorial/ms-identity-ciam-javascript-tutorial-2-call-api-angular/
        }
        private static void addSecuritySimpleJWT1(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
        {
            services.AddAuthentication(options =>
            {

                options.DefaultScheme = "AzureAd";
                options.DefaultAuthenticateScheme = "AzureAd";
                options.DefaultChallengeScheme = "AzureAd";
            })
                .AddJwtBearer("AzureAd", options =>
                {
                    options.Authority = $"https://login.microsoftonline.com/{azureConfig.TenantId}";
                    options.Audience = azureConfig.ClientId; // Ou "api://YOUR_API_ID"
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = $"https://login.microsoftonline.com/{azureConfig.TenantId}/v2.0",
                        ValidAudience = azureConfig.ClientId // Ou "api://YOUR_API_ID"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                            logger.Error("AzureAd Authentication failed.", context.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                            logger.Information("AzureAd Token validated successfully.");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AzureAd", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes("AzureAd");
                });
            });
        }

        private static void addSecuritySimpleJWT(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
        {
            //Funcionou
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://login.microsoftonline.com/{azureConfig.TenantId}";
                    options.Audience = azureConfig.ClientId; // Ou "api://YOUR_API_ID"
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = $"https://login.microsoftonline.com/{azureConfig.TenantId}/v2.0",
                        ValidAudience = azureConfig.ClientId // Ou "api://YOUR_API_ID"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                            logger.Error("AzureAd Authentication failed.", context.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                            logger.Information("AzureAd Token validated successfully.");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                });
            });
        }

        private static void addSecuritySimpleJWT2(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration, AzureAdConfig azureConfig)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer("Bearer", options =>
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
           .AddJwtBearer("AzureAd", options =>
           {
               options.Authority = $"https://login.microsoftonline.com/{azureConfig.TenantId}";
               options.Audience = azureConfig.ClientId; // Ou "api://YOUR_API_ID"
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidIssuer = $"https://login.microsoftonline.com/{azureConfig.TenantId}/v2.0",
                   ValidAudience = azureConfig.ClientId // Ou "api://YOUR_API_ID"
               };
               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                       logger.Error("AzureAd Authentication failed.", context.Exception);
                       return Task.CompletedTask;
                   },
                   OnTokenValidated = context =>
                   {
                       var logger = context.HttpContext.RequestServices.GetRequiredService<Serilog.ILogger>();
                       logger.Information("AzureAd Token validated successfully.");
                       return Task.CompletedTask;
                   }
               };
           });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BearerPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("AzureAdPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes("AzureAd");
                    policy.RequireAuthenticatedUser();
                });
            });
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
