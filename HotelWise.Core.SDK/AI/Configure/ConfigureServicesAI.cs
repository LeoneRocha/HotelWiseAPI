#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.AI.Configure;

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Configure.ConfigureServicesAI. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
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