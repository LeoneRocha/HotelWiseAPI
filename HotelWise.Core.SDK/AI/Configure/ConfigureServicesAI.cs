#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Configuração e registro de serviços de IA no container de injeção de dependência.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Configure.ConfigureServicesAI. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ConfigureServicesAI
{
    /// <summary>
    /// Registra as fábricas e serviços genéricos de IA no container de injeção de dependência.
    /// </summary>
    /// <param name="services">Coleção de serviços de injeção de dependência.</param>
    public static void RegisterGenericAiServices(IServiceCollection services)
    {
        services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
        services.AddScoped<IAIInferenceService, AIInferenceService>();
        services.AddScoped<IVectorStoreAdapterFactory, VectorStoreAdapterFactory>();
    }
}
#endif