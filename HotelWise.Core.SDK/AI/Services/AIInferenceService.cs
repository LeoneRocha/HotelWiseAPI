#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Orquestra chamadas de inferência via fábrica de adapters.
/// </summary>
public class AIInferenceService : IAIInferenceService
{
    private readonly IAIInferenceAdapterFactory _adapterFactory;

    public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
    {
        _adapterFactory = adapterFactory;
    }

    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionAsync(messages);
    }

    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionByAgentAsync(messages);
    }

    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionByAgentSimpleRagAsync(messages);
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateEmbeddingAsync(text);
    }
}
#endif
