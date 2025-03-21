using GroqApiLibrary;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
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
                    ["role"] = getRole(m.RoleType),
                    ["content"] = m.Content
                }).ToArray())
            };

            var result = await _groqApiClient.CreateChatCompletionAsync(request);
            var resultOut = result?["choices"]?[0]?["message"]?["content"]?.ToString();
            return resultOut ?? string.Empty;
        }

        private string getRole(RoleAiPromptsType roleType)
        {
            return roleType switch
            {
                RoleAiPromptsType.System => "system",
                RoleAiPromptsType.Agent => "system",
                RoleAiPromptsType.User => "user",
                RoleAiPromptsType.Assistant => "assistant",
                _ => "user"
            };
        }

        public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages)
        {
            return await GenerateChatCompletionAsync(messages);
        }

        public Task<float[]> GenerateEmbeddingAsync(string text)
        {
            throw new NotImplementedException();
        }
    }
}