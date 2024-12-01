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
            addSecurity(services, tokenConfigurations, configuration);
        }

        private static void addSecurity(IServiceCollection services, TokenConfigurationDto tokenConfigurations, IConfiguration configuration)
        {
            var adScheme = AzureADEntraIDConstants.AzureAd;
            var adSec = configuration.GetSection("AzureAd");

            // Configura a autenticação JWT Bearer e Azure AD em uma única chamada
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            })
            .AddMicrosoftIdentityWebApi(adSec, "AzureAd");

            // Configura a autorização
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
