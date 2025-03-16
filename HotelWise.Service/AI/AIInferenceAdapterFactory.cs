using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;

namespace HotelWise.Service.AI
{
    public class AIInferenceAdapterFactory : IAIInferenceAdapterFactory
    {
        private readonly IApplicationIAConfig _applicationConfig;
        public AIInferenceAdapterFactory(IApplicationIAConfig applicationConfig)
        {
            _applicationConfig = applicationConfig;
        }

        public IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType, IModelStrategy modelStrategy)
        {
            switch (eIAInferenceAdapterType)
            {
                case InferenceAiAdapterType.GroqApi:
                    return new GroqApiAdapter(_applicationConfig, modelStrategy);
                case InferenceAiAdapterType.Mistral:
                    return new MistralApiAdapter(_applicationConfig);
                case InferenceAiAdapterType.Ollama:
                    return new OllamaAdapter(_applicationConfig);
                default:
                    return new MistralApiAdapter(_applicationConfig);
            }
        }
    }
}