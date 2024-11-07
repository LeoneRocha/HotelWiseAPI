using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces;

namespace HotelWise.Service.AI
{
    public class AIInferenceAdapterFactory : IAIInferenceAdapterFactory
    {
        public IAIInferenceAdapter CreateAdapter(string apiKey, IModelStrategy modelStrategy)
        {
            return new GroqApiAdapter(apiKey, modelStrategy);
        }
    }
}