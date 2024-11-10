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
        private readonly IVectorStoreSettingsDto _vectorStoreSettingsDto;
        public AIInferenceAdapterFactory(IConfiguration configuration, IVectorStoreSettingsDto vectorStoreSettingsDto)
        {
            _configuration = configuration;
            _vectorStoreSettingsDto = vectorStoreSettingsDto;
        }

        public IAIInferenceAdapter CreateAdapter(EIAInferenceAdapterType eIAInferenceAdapterType, IModelStrategy modelStrategy)
        {
            switch (eIAInferenceAdapterType)
            {
                case EIAInferenceAdapterType.GroqApi:
                    return new GroqApiAdapter(_configuration, modelStrategy);
                case EIAInferenceAdapterType.Mistral:
                    return new MistralApiAdapter(_vectorStoreSettingsDto);
                default:
                    return new MistralApiAdapter(_vectorStoreSettingsDto);
            }
        }
    }
}