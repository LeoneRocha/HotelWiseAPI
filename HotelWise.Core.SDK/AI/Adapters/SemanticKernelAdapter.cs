#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Semantic Kernel — casca sobre SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.SemanticKernelAdapter", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.SemanticKernelAdapter em SmartCoreHub.Core.SDK.")]
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
        _inner = new SchAdapters.SemanticKernelAdapter(applicationConfig, serviceProvider);
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
