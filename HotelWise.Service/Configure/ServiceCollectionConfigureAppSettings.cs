﻿using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelWise.Service.Configure
{
    public static class ServiceCollectionConfigureAppSettings
    {
        public static void Configure(IServiceCollection services, IConfiguration _configuration)
        {
            addAzureAdConfig(services, _configuration); 
        }


        private static void addAzureAdConfig(IServiceCollection services, IConfiguration configuration)
        {
            // Bind the PolicyConfig section of appsettings.json to the PolicyConfig class
            var appValue = new AzureAdConfig();

            var configValue = ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
            new ConfigureFromConfigurationOptions<AzureAdConfig>(configValue)
             .Configure(appValue);
            // Register the PolicyConfig instance as a singleton
            services.AddSingleton<IAzureAdConfig>(appValue);
        }

        public static TokenConfigurationDto AddAndReturnTokenConfiguration(IServiceCollection services, IConfiguration _configuration)
        {
            var configValue = ConfigurationAppSettingsHelper.GetTokenConfigurations(_configuration);

            var tokenConfigurations = new TokenConfigurationDto();

            new ConfigureFromConfigurationOptions<TokenConfigurationDto>(configValue)
             .Configure(tokenConfigurations);

            services.AddSingleton<ITokenConfigurationDto>(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            return tokenConfigurations;
        }

     
      

        
    }
}
