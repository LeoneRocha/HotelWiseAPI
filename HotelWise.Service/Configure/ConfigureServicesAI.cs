using HotelWise.Data.Repository;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Service.AI;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    public static class ConfigureServicesAI
    {
        public static void ConfigureServices(IServiceCollection services)
        {  
            services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
            services.AddScoped<IAIInferenceService, AIInferenceService>();
            services.AddScoped<IGenerateHotelService, GenerateHotelService>();

            services.AddScoped<IVectorStoreAdapterFactory, VectorStoreAdapterFactory>();

            services.AddScoped<IVectorStoreService<HotelVector>, HotelVectorStoreService>();

            services.AddScoped<IAssistantService, AssistantService>(); 
        }
    }
}