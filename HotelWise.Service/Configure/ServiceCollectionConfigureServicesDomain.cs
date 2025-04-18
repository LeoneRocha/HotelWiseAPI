﻿using FluentValidation;
using HotelWise.Domain.Interfaces.AppConfig;
using HotelWise.Domain.Validator.HotelValidators;
using HotelWise.Service.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelWise.Service.Configure
{
    public static class ServiceCollectionConfigureServicesDomain
    {
        public static void Configure(IServiceCollection services, IConfiguration _configuration)
        {
            ////AutoMapper
            ServiceCollectionConfigureAutoMapper.Configure(services);
            addDependenciesSingleton(services);
              
            ServicesDomainRepository.AddDependenciesManually(services);

            ServicesDomainService.AddDependenciesManually(services);
             
            ConfigureServicesAI.ConfigureServices(services);
             
            #region KERNEL  
            SemanticKernelProviderConfigure.SetupSemanticKernelProvider(services, _configuration);
            #endregion KERNEL
   
            //Validators
            services.AddValidatorsFromAssemblyContaining<HotelValidator>();

            ServicesDomainRepository.AddDependenciesAuto(services);
            ServicesDomainService.AddDependenciesAuto(services);
        } 
        private static void addDependenciesSingleton(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ITokenService, TokenService>(); 
        }
    }
}
