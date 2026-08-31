#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Mistral API — casca sobre SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.MistralApiAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.MistralApiAdapter _inner;

    public MistralApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.MistralApiAdapter(ApplicationIAConfigSchBridge.ToSch(applicationConfig));
    }

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
