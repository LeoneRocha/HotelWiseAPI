using FluentValidation;
using HotelWise.Data.Repository;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Validator;
using HotelWise.Service.AI;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    public static class ConfigureServicesAI
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddValidatorsFromAssemblyContaining<HotelValidator>();

            services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
            services.AddScoped<IAIInferenceService, AIInferenceService>();
            services.AddScoped<IGenerateHotelService, GenerateHotelService>();

            services.AddScoped<IVectorStoreAdapterFactory, VectorStoreAdapterFactory>();
            
            services.AddScoped<IVectorStoreService<HotelVector>, HotelVectorStoreService>();            
        }
    }
}