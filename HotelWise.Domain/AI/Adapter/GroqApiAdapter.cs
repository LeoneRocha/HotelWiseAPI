using GroqApiLibrary;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using System.Text.Json.Nodes;

namespace HotelWise.Domain.AI.Adapter
{
    public class GroqApiAdapter : IAIInferenceAdapter
    {
        private readonly GroqApiClient _groqApiClient;
        private readonly string _model; 
         
        public GroqApiAdapter(IApplicationIAConfig applicationConfig)
        {
            _groqApiClient = new GroqApiClient(applicationConfig.GroqApiConfig.ApiKey);
            _model = applicationConfig.GroqApiConfig.ModelId;
        }

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
        { 
            var request = new JsonObject
            {
                ["model"] = _model,
                ["messages"] = new JsonArray(messages.Select(m => new JsonObject
                {
                    ["role"] = m.Role,
                    ["content"] = m.Content
                }).ToArray())
            };

            var result = await _groqApiClient.CreateChatCompletionAsync(request);
            var resultOut = result?["choices"]?[0]?["message"]?["content"]?.ToString();
            return resultOut ?? string.Empty;
        } 

        public Task<float[]> GenerateEmbeddingAsync(string text)
        {
            throw new NotImplementedException();
        }
    }
}