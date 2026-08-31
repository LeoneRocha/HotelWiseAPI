#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Groq API — casca sobre SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.GroqApiAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GroqApiAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.GroqApiAdapter _inner;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="GroqApiAdapter"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração da aplicação IA (SCH via <see cref="IApplicationIAConfig"/>).</param>
    public GroqApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.GroqApiAdapter(applicationConfig);
    }

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionAsync(messages);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentAsync(messages);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentSimpleRagAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentSimpleRagAsync(messages);

    /// <inheritdoc />
    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        _inner.GenerateEmbeddingAsync(text);
}
#endif
