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

        public GroqApiAdapter(IApplicationIAConfig applicationConfig, IModelStrategy modelStrategy)
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
                        ["role"] = "system",
                        ["content"] = "Você é um assistente de viagens e turismo. Você só responde a perguntas relacionadas a viagens, reservas de hotéis e turismo. Se a pergunta estiver fora desse escopo, responda de forma objetiva que não pode ajudar com isso. Não forneca nehuma infomação fora do escopo sobre viagens, reservas de hotéis e turismo"
                    },
                      new JsonObject
                    {
                        ["role"] = "system",
                        ["content"] = "Só responda exclusivamente em tópicos relacionados a viagens e turismo, e a responder de forma respeitosa e breve quando a pergunta estiver fora desse escopo"
                    },
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