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

        public IAIInferenceAdapter CreateAdapter(AInferenceAdapterType eIAInferenceAdapterType, IModelStrategy modelStrategy)
        {
            switch (eIAInferenceAdapterType)
            {
                case AInferenceAdapterType.GroqApi:
                    return new GroqApiAdapter(_applicationConfig, modelStrategy);
                case AInferenceAdapterType.Mistral:
                    return new MistralApiAdapter(_applicationConfig);
                default:
                    return new MistralApiAdapter(_applicationConfig);
            }
        }
    }
}