using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;

namespace HotelWise.Domain.AI.Adapter
{
#pragma warning disable SKEXP0001
    public class SemanticKernelAdapter : IAIInferenceAdapter
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ITextEmbeddingGenerationService _embeddingGenerationService;

        public SemanticKernelAdapter(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
        {
            var kernel = serviceProvider.GetRequiredService<Kernel>();
            var config = applicationConfig.GetChatServiceConfig()
                        ?? throw new InvalidOperationException("Semantic Kernel configuration is missing or invalid.");

            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            _embeddingGenerationService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        }
        
        //FEATURE CREAT CHAT BY AGENT 

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
        {
            if (messages == null || messages.Length <= 0)
                throw new ArgumentException("Messages cannot be null or empty.");

            var chatHistory = new ChatHistory();
            foreach (var message in messages)
            {
                if (message.RoleType == RoleAiPromptsType.System)
                {
                    // Define as instruções ou o contexto para guiar o comportamento da IA.
                    chatHistory.AddSystemMessage(message.Content);
                } 
                if (message.RoleType == RoleAiPromptsType.Assistant)
                {
                    // Adiciona uma resposta gerada pelo assistente ao histórico da conversa.
                    chatHistory.AddAssistantMessage(message.Content);
                } 
                if (message.RoleType == RoleAiPromptsType.User)
                {
                    // Adiciona a entrada fornecida pelo usuário ao histórico da conversa.
                    chatHistory.AddUserMessage(message.Content); 
                } 
            }
            var result = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);

            if (result == null)
            {
                return string.Empty;
            }
            var resultString = result.Content;
            resultString = MarkdownHelper.ConvertToHtmlIfMarkdown(resultString ?? string.Empty);
            return resultString;
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or empty.");

            var embeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync(new List<string> { text });

            if (embeddings == null || embeddings.Count == 0 || embeddings[0].Length == 0)
            {
                return Array.Empty<float>();
            }

            return embeddings[0].ToArray();
        }
    }
#pragma warning restore SKEXP0001
}