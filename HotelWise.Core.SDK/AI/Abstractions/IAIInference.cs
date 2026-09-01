using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de inferência LLM (chat e embeddings).
/// </summary>
public interface IAIInferenceAdapter
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter
{
}

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// </summary>
public interface IAIInferenceAdapterFactory
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory
{
}

/// <summary>
/// Serviço de orquestração de inferência.
/// </summary>
public interface IAIInferenceService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService
{
}

/// <summary>
/// Serviço de assistente conversacional voltado ao usuário final.
/// </summary>
public interface IAssistantService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService
{
}
