using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Service.AI;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    /// <summary>
    /// DI de IA: regs genéricos via Core + serviços hotel (vector store, generate, assistant).
    /// </summary>
    public static class ConfigureServicesAI
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            HotelWise.Core.SDK.AI.Configure.ConfigureServicesAI.RegisterGenericAiServices(services);

            services.AddScoped<IGenerateHotelService, GenerateHotelService>();
            services.AddScoped<IVectorStoreService<HotelVector>, HotelVectorStoreService>();
            services.AddScoped<IAssistantService, AssistantService>();
        }
    }
}
