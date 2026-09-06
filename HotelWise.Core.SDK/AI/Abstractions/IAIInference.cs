using HotelWise.Core.SDK.AI.DTO;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de inferência LLM (chat e embeddings).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter em SmartCoreHub.Core.SDK.")]
public interface IAIInferenceAdapter
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter
{
}

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory em SmartCoreHub.Core.SDK.")]
public interface IAIInferenceAdapterFactory
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory
{
}

/// <summary>
/// Serviço de orquestração de inferência.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService em SmartCoreHub.Core.SDK.")]
public interface IAIInferenceService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService
{
}

/// <summary>
/// Serviço de assistente conversacional voltado ao usuário final.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService em SmartCoreHub.Core.SDK.")]
public interface IAssistantService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService
{
}
