using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.AppConfig;
using HotelWise.Domain.Interfaces.IA;
using Mistral.SDK;
using Mistral.SDK.DTOs;

namespace HotelWise.Domain.AI.Adapter
{
    public class MistralApiAdapter : IAIInferenceAdapter
    { 
        private readonly MistralClient _client;

        public MistralApiAdapter(IApplicationIAConfig applicationConfig)
        { 
            _client = new MistralClient(applicationConfig.MistralApiConfig.ApiKey);
        }
        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
        {
            var chatMessages = messages.Select(m => new ChatMessage(
                getRole(m),
                m.Content)).ToList();

            var request = new ChatCompletionRequest(
                model: ModelDefinitions.MistralMedium,
                messages: chatMessages,
                safePrompt: true,
                temperature: 0,
                maxTokens: 500,
                topP: 1,
                randomSeed: 32
            );

            var response = await _client.Completions.GetCompletionAsync(request);
            return response.VarObject.ToString();
        }

        private static ChatMessage.RoleEnum getRole(PromptMessageVO pm)
        {
            switch (pm.RoleType)
            {
                case Enuns.IA.RoleAiPromptsType.System:
                case Enuns.IA.RoleAiPromptsType.Agent:                    
                    return ChatMessage.RoleEnum.System;
                case Enuns.IA.RoleAiPromptsType.User:
                    return ChatMessage.RoleEnum.User;
                case Enuns.IA.RoleAiPromptsType.Assistant:
                    return ChatMessage.RoleEnum.Assistant;
             
                default:
                    return ChatMessage.RoleEnum.User;
            }
        }

        public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages)
        {
            return await GenerateChatCompletionAsync(messages);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var request = new EmbeddingRequest(ModelDefinitions.MistralEmbed, new List<string>() { text }, EmbeddingRequest.EncodingFormatEnum.Float);

            var response = await _client.Embeddings.GetEmbeddingsAsync(request);
              
            var resultEmbedding = new List<float>();
            response.Data.ForEach(x => resultEmbedding.AddRange(x.Embedding.Select(eb=> (float)eb).ToList()));

            return resultEmbedding.ToArray();
        }

        public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages)
        {
            return await GenerateChatCompletionAsync(messages);
        }
    }
}