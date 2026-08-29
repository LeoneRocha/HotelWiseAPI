using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de inferência LLM.
/// </summary>
public interface IAIInferenceAdapter
{
    Task<float[]> GenerateEmbeddingAsync(string text);
    Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages);
    Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages);
    Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages);
}

/// <summary>
/// Fábrica de adapters de inferência.
/// </summary>
public interface IAIInferenceAdapterFactory
{
    IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType);
}

/// <summary>
/// Serviço de orquestração de inferência.
/// </summary>
public interface IAIInferenceService
{
    Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
    Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
    Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
    Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType);
}

/// <summary>
/// Serviço de assistente conversacional.
/// </summary>
public interface IAssistantService
{
    Task<float[]?> GenerateEmbeddingAsync(string text);
    Task<AskAssistantResponse[]?> AskAssistant(AskAssistantRequest request);
    void SetUserId(long id);
}
