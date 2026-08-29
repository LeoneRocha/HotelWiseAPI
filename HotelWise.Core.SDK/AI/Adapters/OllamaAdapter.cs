#if NET8_0_OR_GREATER
using System.Text;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Helpers;
using OllamaSharp;
using OllamaSharp.Models.Chat;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Ollama.
/// </summary>
public class OllamaAdapter : IAIInferenceAdapter
{
    private readonly OllamaApiClient _clientChat;
    private readonly OllamaApiClient _clientEmbedding;

    public OllamaAdapter(IApplicationIAConfig applicationConfig)
    {
        var config = applicationConfig.GetChatServiceConfig(AIChatServiceType.OllamaAdapter) as OllamaConfig
                ?? throw new InvalidOperationException("Ollama configuration is missing.");
        _clientChat = CreateClient(config.Endpoint, config.ModelId);
        _clientEmbedding = CreateClient(config.Endpoint, config.ModelId);
    }

    public OllamaApiClient GetClientChat() => _clientChat;
    public OllamaApiClient GetClientEmbedding() => _clientEmbedding;

    private static OllamaApiClient CreateClient(string url, string modelId)
    {
        var uri = new Uri(url);
        var clientInstance = new OllamaApiClient(uri);
        clientInstance.SelectedModel = modelId;
        return clientInstance;
    }

    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        if (messages == null || messages.Length <= 0)
            throw new ArgumentException("Messages cannot be null or empty.");

        ChatRequest chatRequest = new ChatRequest();
        var chatMessages = messages.Select(m => new Message
        {
            Role = ConvertRole(m.Role),
            Content = m.Content
        }).ToList();

        chatRequest.Messages = chatMessages;
        StringBuilder responseContent = new StringBuilder();

        await foreach (var stream in _clientChat.ChatAsync(chatRequest))
        {
            var msgresult = stream!.Message.Content;
            if (msgresult != null)
            {
                responseContent.Append(msgresult);
            }
        }

        return MarkdownHelper.ConvertToHtmlIfMarkdown(responseContent.ToString());
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.");

        var embedding = await _clientChat.EmbedAsync(text, CancellationToken.None);
        return embedding.Embeddings?.FirstOrDefault()?.ToArray() ?? Array.Empty<float>();
    }

    private static string ConvertRole(string role) =>
        role.ToLower() switch
        {
            "agent" => "system",
            "system" => "system",
            "user" => "user",
            "assistant" => "assistant",
            _ => "user"
        };

    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);

    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);
}
#endif
