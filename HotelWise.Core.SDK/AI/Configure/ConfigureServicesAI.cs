#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Registros DI genéricos de IA (sem serviços de domínio hotel).
/// Registra fábricas e o serviço de orquestração de inferência
/// (<see cref="IAIInferenceAdapterFactory"/>, <see cref="IAIInferenceService"/>,
/// <see cref="IVectorStoreAdapterFactory"/>).
/// </summary>
/// <example>
/// <code>
/// // Em Program.cs, após SetupSemanticKernelProvider
/// ConfigureServicesAI.RegisterGenericAiServices(builder.Services);
///
/// // Resolução
/// var inference = sp.GetRequiredService&lt;IAIInferenceService&gt;();
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Configure.ConfigureServicesAI. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ConfigureServicesAI
{
    /// <summary>
    /// Registra no DI as fábricas e o serviço genérico de inferência/vector store.
    /// </summary>
    /// <param name="services">Coleção de serviços DI.</param>
    /// <example>
    /// <code>
    /// ConfigureServicesAI.RegisterGenericAiServices(services);
    /// </code>
    /// </example>
    public static void RegisterGenericAiServices(IServiceCollection services)
    {
        services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
        services.AddScoped<IAIInferenceService, AIInferenceService>();
        services.AddScoped<IVectorStoreAdapterFactory, VectorStoreAdapterFactory>();
    }
}
#endif
