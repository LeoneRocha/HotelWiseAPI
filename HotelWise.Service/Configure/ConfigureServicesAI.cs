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
    /// <summary>
    /// ⚠️ Parte genérica no Core (RegisterGenericAiServices). Host mantém regs hotel.
    /// </summary>
    [Obsolete(
        "Regs genéricos: HotelWise.Core.SDK.AI.Configure.ConfigureServicesAI.RegisterGenericAiServices. Host mantém serviços de domínio.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
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
