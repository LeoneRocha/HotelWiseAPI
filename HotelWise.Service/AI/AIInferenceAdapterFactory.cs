using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Service.AI
{
    public class AIInferenceAdapterFactory : IAIInferenceAdapterFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationConfig _applicationConfig;
        public AIInferenceAdapterFactory(IConfiguration configuration, IApplicationConfig applicationConfig)
        {
            _configuration = configuration;
            _applicationConfig = applicationConfig;
        }

        public IAIInferenceAdapter CreateAdapter(EIAInferenceAdapterType eIAInferenceAdapterType, IModelStrategy modelStrategy)
        {
            switch (eIAInferenceAdapterType)
            {
                case EIAInferenceAdapterType.GroqApi:
                    return new GroqApiAdapter(_configuration, modelStrategy);
                case EIAInferenceAdapterType.Mistral:
                    return new MistralApiAdapter(_applicationConfig);
                default:
                    return new MistralApiAdapter(_applicationConfig);
            }
        }
    }
}