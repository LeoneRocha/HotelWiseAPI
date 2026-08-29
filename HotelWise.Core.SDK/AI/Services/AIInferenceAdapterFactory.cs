#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// </summary>
public class AIInferenceAdapterFactory : IAIInferenceAdapterFactory
{
    private readonly IApplicationIAConfig _applicationConfig;
    private readonly IServiceProvider _serviceProvider;

    public AIInferenceAdapterFactory(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
    {
        _applicationConfig = applicationConfig;
        _serviceProvider = serviceProvider;
    }

    public IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType)
    {
        return eIAInferenceAdapterType switch
        {
            InferenceAiAdapterType.GroqApi => new GroqApiAdapter(_applicationConfig),
            InferenceAiAdapterType.Mistral => new MistralApiAdapter(_applicationConfig),
            InferenceAiAdapterType.Ollama => new OllamaAdapter(_applicationConfig),
            InferenceAiAdapterType.SemanticKernel => new SemanticKernelAdapter(_applicationConfig, _serviceProvider),
            _ => new GroqApiAdapter(_applicationConfig)
        };
    }
}
#endif
