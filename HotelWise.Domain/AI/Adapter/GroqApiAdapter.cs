using GroqApiLibrary;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using System.Text.Json.Nodes;

namespace HotelWise.Domain.AI.Adapter
{
    public class GroqApiAdapter : IAIInferenceAdapter
    {
        private readonly GroqApiClient _groqApiClient;
        private readonly IModelStrategy _modelStrategy; 

        public GroqApiAdapter(IApplicationConfig applicationConfig, IModelStrategy modelStrategy)
        { 
            _groqApiClient = new GroqApiClient(applicationConfig.GroqApiConfig.ApiKey);
            _modelStrategy = modelStrategy;
        }
        public async Task<string> GenerateChatCompletionAsync(string prompt)
        {
            var model = _modelStrategy.GetModel();
            var request = new JsonObject
            {
                ["model"] = model,
                ["messages"] = new JsonArray
            {
                new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = prompt
                }
            }
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