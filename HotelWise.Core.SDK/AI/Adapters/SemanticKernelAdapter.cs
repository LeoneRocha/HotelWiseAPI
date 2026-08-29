#if NET8_0_OR_GREATER
using System.Text;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Semantic Kernel.
/// </summary>
public class SemanticKernelAdapter : IAIInferenceAdapter
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private const string LineSeparator = "--------------------------------";

    public SemanticKernelAdapter(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
    {
        _kernel = serviceProvider.GetRequiredService<Kernel>()
            ?? throw new InvalidOperationException("Kernel não foi inicializado corretamente.");
        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        _embeddingGenerator = _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
    }

    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        var resultInference = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference?.Content);
    }

    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));
        var resultInference = await ProcessAgentResultAsync(agent, chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference);
    }

    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        AddContextToChatHistory(chatHistory, messages);
        var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));
        var resultInference = await ProcessAgentResultAsync(agent, chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference);
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.");

        var embedding = await _embeddingGenerator.GenerateAsync(text);
        if (embedding.Vector.Length == 0)
            return Array.Empty<float>();
        return embedding.Vector.ToArray();
    }

    private static void ValidateMessages(PromptMessageVO[] messages)
    {
        if (messages == null || messages.Length == 0)
            throw new ArgumentException("Messages cannot be null or empty.");
    }

    private static string ProcessResultContentToHtmlIfMarkdown(string? content) =>
        MarkdownHelper.ConvertToHtmlIfMarkdown(content ?? string.Empty);

    private static async Task<string> ProcessAgentResultAsync(ChatCompletionAgent agent, ChatHistory chatHistory)
    {
        var resultBuilder = new StringBuilder();
        await foreach (var message in agent.InvokeAsync(chatHistory))
        {
            resultBuilder.Append(message.Message.Content);
        }
        return resultBuilder.ToString();
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

    private ChatCompletionAgent BuildAgent(PromptMessageVO agentMessage) =>
        new ChatCompletionAgent
        {
            Instructions = agentMessage.Content,
            Name = agentMessage.AgentName,
            Kernel = _kernel
        };

    private static void AddContextToChatHistory(ChatHistory chatHistory, PromptMessageVO[] messages)
    {
        var contextMessage = messages.FirstOrDefault(m => m.RoleType == RoleAiPromptsType.Context);
        if (contextMessage?.DataContextRag == null) return;

        var contextBuilder = new StringBuilder();
        foreach (var item in contextMessage.DataContextRag)
        {
            contextBuilder.AppendLine(item.DataVector);
            contextBuilder.AppendLine(LineSeparator);
        }
        chatHistory.AddUserMessage($"Context:\n\n{contextBuilder}");
    }
}
#endif
