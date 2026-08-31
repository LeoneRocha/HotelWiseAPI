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

    /// <summary>
    /// Inicializa uma nova instância de <see cref="OllamaAdapter"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração da aplicação IA.</param>
    public OllamaAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.OllamaAdapter(ApplicationIAConfigSchBridge.ToSch(applicationConfig));
    }

    /// <summary>
    /// Obtém o cliente de chat do Ollama.
    /// </summary>
    /// <returns>Instância de <see cref="OllamaSharp.OllamaApiClient"/> para chat.</returns>
    public OllamaSharp.OllamaApiClient GetClientChat() => _inner.GetClientChat();

    /// <summary>
    /// Obtém o cliente de embedding do Ollama.
    /// </summary>
    /// <returns>Instância de <see cref="OllamaSharp.OllamaApiClient"/> para embeddings.</returns>
    public OllamaSharp.OllamaApiClient GetClientEmbedding() => _inner.GetClientEmbedding();

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionAsync(messages);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentAsync(messages);

    /// <inheritdoc />
    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        _inner.GenerateEmbeddingAsync(text);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentSimpleRagAsync(messages);
}
#endif
