#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Registros DI genéricos de IA (sem serviços de domínio hotel).
/// </summary>
public static class ConfigureServicesAI
{
    public static void RegisterGenericAiServices(IServiceCollection services)
    {
        services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
        services.AddScoped<IAIInferenceService, AIInferenceService>();
        services.AddScoped<IVectorStoreAdapterFactory, VectorStoreAdapterFactory>();
    }
}
#endif
