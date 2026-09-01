#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using SchFactory = SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceAdapterFactory;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de inferência LLM — herda SCH.
/// </summary>
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
