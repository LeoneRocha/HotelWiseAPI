using HotelWise.Domain.AI.Models;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Service.AI
{
    public class AIInferenceService : IAIInferenceService
    {
        private EIAInferenceAdapterType _eIAInferenceAdapterType;
        private readonly IAIInferenceAdapterFactory _adapterFactory;

        public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
        {
            _eIAInferenceAdapterType = EIAInferenceAdapterType.Mistral;
            _adapterFactory = adapterFactory;
        }

        public async Task<string> GenerateChatCompletionAsync(string prompt, EIAInferenceAdapterType eIAInferenceAdapterType)
        {
            var model = new MixtralModelStrategy();
            var _adapter = _adapterFactory.CreateAdapter(_eIAInferenceAdapterType, model);

            return await _adapter!.GenerateChatCompletionAsync(prompt);
        }

        public async Task<decimal[]> GenerateEmbeddingAsync(string text, EIAInferenceAdapterType eIAInferenceAdapterType)
        {
            var model = new MixtralModelStrategy();
            var _adapter = _adapterFactory.CreateAdapter(_eIAInferenceAdapterType, model);

            return await _adapter!.GenerateEmbeddingAsync(text);
        }
    }
}