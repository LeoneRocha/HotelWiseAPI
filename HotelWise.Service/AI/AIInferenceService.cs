using HotelWise.Domain.AI.Models;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Service.AI
{
    public class AIInferenceService : IAIInferenceService
    { 
        private readonly IAIInferenceAdapterFactory _adapterFactory;

        public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
        { 
            _adapterFactory = adapterFactory;
        }

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, IAInferenceAdapterType eIAInferenceAdapterType)
        {
            var model = new MixtralModelStrategy(); 
            var _adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType, model);
            return await _adapter!.GenerateChatCompletionAsync(messages);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text, IAInferenceAdapterType eIAInferenceAdapterType)
        {
            var model = new MixtralModelStrategy(); 
            var _adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType, model);
            return await _adapter!.GenerateEmbeddingAsync(text);
        }
    }
}