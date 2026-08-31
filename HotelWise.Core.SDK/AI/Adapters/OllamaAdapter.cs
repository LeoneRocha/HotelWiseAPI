#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Ollama — casca sobre SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.OllamaAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OllamaAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.OllamaAdapter _inner;

    public OllamaAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.OllamaAdapter(ApplicationIAConfigSchBridge.ToSch(applicationConfig));
    }

    public OllamaSharp.OllamaApiClient GetClientChat() => _inner.GetClientChat();

    public OllamaSharp.OllamaApiClient GetClientEmbedding() => _inner.GetClientEmbedding();

    public Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionAsync(messages);

    public Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentAsync(messages);

    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        _inner.GenerateEmbeddingAsync(text);

    public Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentSimpleRagAsync(messages);
}
#endif
