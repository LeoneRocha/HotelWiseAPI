#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Semantic Kernel — casca sobre SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.SemanticKernelAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SemanticKernelAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.SemanticKernelAdapter _inner;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="SemanticKernelAdapter"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração da aplicação IA.</param>
    /// <param name="serviceProvider">Provedor de serviços para injeção de dependência.</param>
    public SemanticKernelAdapter(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
    {
        _inner = new SchAdapters.SemanticKernelAdapter(ApplicationIAConfigSchBridge.ToSch(applicationConfig), serviceProvider);
    }

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
