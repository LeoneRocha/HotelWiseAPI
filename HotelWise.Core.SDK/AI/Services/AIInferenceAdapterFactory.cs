#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using SchFactory = SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceAdapterFactory;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de inferência LLM — herda SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AIInferenceAdapterFactory : SchFactory, IAIInferenceAdapterFactory
{
    /// <summary>
    /// Inicializa a fábrica delegando configuração SCH.
    /// </summary>
    public AIInferenceAdapterFactory(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
        : base(applicationConfig, serviceProvider)
    {
    }
}
#endif
