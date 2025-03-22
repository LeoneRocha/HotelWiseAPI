using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using System.Text;

namespace HotelWise.Domain.AI.Adapter
{
#pragma warning disable SKEXP0001
    public class SemanticKernelAdapter : IAIInferenceAdapter
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ITextEmbeddingGenerationService _embeddingGenerationService;
        private const string LineSeparator = "--------------------------------";
        public SemanticKernelAdapter(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
        {
            _kernel = serviceProvider.GetRequiredService<Kernel>()
                ?? throw new InvalidOperationException("Kernel não foi inicializado corretamente.");
            _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            _embeddingGenerationService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        }

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
        {
            ValidateMessages(messages);

            var chatHistory = BuildChatHistory(messages);

            var resultInference = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);

            var resultChat = ProcessResultContentToHtmlIfMarkdown(resultInference?.Content);

            return resultChat;
        }
        public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages)
        {
            ValidateMessages(messages);

            var chatHistory = BuildChatHistory(messages);

            var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));

            var resultInference = await ProcessAgentResultAsync(agent, chatHistory);

            var resultChat = ProcessResultContentToHtmlIfMarkdown(resultInference);

            return resultChat;
        }

        public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages)
        {
            ValidateMessages(messages);

            var chatHistory = BuildChatHistory(messages);

            AddContextToChatHistory(chatHistory, messages);

            var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));

            var resultInference = await ProcessAgentResultAsync(agent, chatHistory);

            var resultChat = ProcessResultContentToHtmlIfMarkdown(resultInference);

            return resultChat;
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

        #region PRIVATE

        private static void ValidateMessages(PromptMessageVO[] messages)
        {
            if (messages == null || messages.Length == 0)
                throw new ArgumentException("Messages cannot be null or empty.");
        }

        private static string ProcessResultContentToHtmlIfMarkdown(string content)
        {
            return MarkdownHelper.ConvertToHtmlIfMarkdown(content ?? string.Empty);
        }

        private async Task<string> ProcessAgentResultAsync(ChatCompletionAgent agent, ChatHistory chatHistory)
        {
            var resultBuilder = new StringBuilder();

            await foreach (var message in agent.InvokeAsync(chatHistory))
            {
                resultBuilder.Append(message.Content);
            }

            var result = resultBuilder.ToString();
            return result;
        }

        private static ChatHistory BuildChatHistory(PromptMessageVO[] messages)
        {
            var chatHistory = new ChatHistory();
            foreach (var message in messages)
            {
                switch (message.RoleType)
                {
                    case RoleAiPromptsType.System:
                        chatHistory.AddSystemMessage(message.Content);
                        break;
                    case RoleAiPromptsType.Assistant:
                        chatHistory.AddAssistantMessage(message.Content);
                        break;
                    case RoleAiPromptsType.User:
                        chatHistory.AddUserMessage(message.Content);
                        break;
                }
            }
            return chatHistory;
        }

        private ChatCompletionAgent BuildAgent(PromptMessageVO agentMessage)
        {
            return new ChatCompletionAgent
            {
                Instructions = agentMessage.Content,
                Name = agentMessage.AgentName,
                Kernel = _kernel
            };
        }

        private static void AddContextToChatHistory(ChatHistory chatHistory, PromptMessageVO[] messages)
        {
            var contextMessage = messages.FirstOrDefault(m => m.RoleType == RoleAiPromptsType.Context);

            if (contextMessage?.DataContextRag != null)
            {
                var contextBuilder = new StringBuilder();

                foreach (var item in contextMessage.DataContextRag)
                {
                    contextBuilder.AppendLine(item.DataVector);
                    contextBuilder.AppendLine(LineSeparator);
                }

                chatHistory.AddUserMessage($"Context:\n\n{contextBuilder}");
            }
        }

        #endregion PRIVATE
    }
}
