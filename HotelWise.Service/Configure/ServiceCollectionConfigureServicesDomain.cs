using FluentValidation;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Validator;
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

            addCollectionDependencies(services);

            ConfigureServicesAI.ConfigureServices(services);

            ServicesDomainService.AddDependenciesManually(services);
             
            #region KERNEL  
            SemanticKernelProviderConfigure.SetupSemanticKernelProvider(services, _configuration);
            #endregion KERNEL
   
            //Validators
            services.AddValidatorsFromAssemblyContaining<HotelValidator>();

            ServicesDomainRepository.AddDependencies(services);
            ServicesDomainService.AddDependenciesAuto(services);
        }
        private static void addCollectionDependencies(IServiceCollection services)
        {
           
        }
        private static void addDependenciesSingleton(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ITokenService, TokenService>(); 
        }
    }
}
