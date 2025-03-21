using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
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

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
        {  
            var _adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await _adapter!.GenerateChatCompletionAsync(messages);
        }

        public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
        {
            var _adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await _adapter!.GenerateChatCompletionByAgentAsync(messages);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType)
        { 
            var _adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await _adapter!.GenerateEmbeddingAsync(text);
        }
    }
}