using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.AI
{
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
            switch (eIAInferenceAdapterType)
            {
                case InferenceAiAdapterType.GroqApi:
                    return new GroqApiAdapter(_applicationConfig);
                case InferenceAiAdapterType.Mistral:
                    return new MistralApiAdapter(_applicationConfig);
                case InferenceAiAdapterType.Ollama:
                    return new OllamaAdapter(_applicationConfig);
                case InferenceAiAdapterType.SemanticKernel:
                    return new SemanticKernelAdapter(_applicationConfig, _serviceProvider);
                default:
                    return new GroqApiAdapter(_applicationConfig);
            }
        }
    }
}