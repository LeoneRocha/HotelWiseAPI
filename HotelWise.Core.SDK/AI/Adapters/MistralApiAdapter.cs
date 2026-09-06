#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Mistral API — casca sobre SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.MistralApiAdapter", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.MistralApiAdapter em SmartCoreHub.Core.SDK.")]
public class MistralApiAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.MistralApiAdapter _inner;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="MistralApiAdapter"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração da aplicação IA.</param>
    public MistralApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.MistralApiAdapter(applicationConfig);
    }

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionAsync(messages);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentAsync(messages);

    /// <inheritdoc />
    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        _inner.GenerateEmbeddingAsync(text);

    /// <inheritdoc />
    public Task<string> GenerateChatCompletionByAgentSimpleRagAsync(SchDto.PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentSimpleRagAsync(messages);
}
#endif
