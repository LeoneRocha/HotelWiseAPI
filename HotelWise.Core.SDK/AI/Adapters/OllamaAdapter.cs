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
/// Adapter de inferência via Ollama (inferência local).
/// Implementa <see cref="IAIInferenceAdapter"/> usando <see cref="OllamaApiClient"/>
/// para chat streaming e embeddings, com conversão de roles do pipeline HotelWise.
/// </summary>
/// <example>
/// <code>
/// var adapter = new OllamaAdapter(appConfig);
/// string reply = await adapter.GenerateChatCompletionAsync(messages);
/// float[] emb = await adapter.GenerateEmbeddingAsync("texto");
/// </code>
/// </example>
public class OllamaAdapter : IAIInferenceAdapter
{
    /// <summary>
    /// Cliente Ollama usado para chat.
    /// </summary>
    private readonly OllamaApiClient _clientChat;

    /// <summary>
    /// Cliente Ollama usado para embeddings.
    /// </summary>
    private readonly OllamaApiClient _clientEmbedding;

    /// <summary>
    /// Inicializa os clientes Ollama a partir da configuração <see cref="AIChatServiceType.OllamaAdapter"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA.</param>
    public OllamaAdapter(IApplicationIAConfig applicationConfig)
    {
        var config = applicationConfig.GetChatServiceConfig(AIChatServiceType.OllamaAdapter) as OllamaConfig
                ?? throw new InvalidOperationException("Ollama configuration is missing.");
        _clientChat = CreateClient(config.Endpoint, config.ModelId);
        _clientEmbedding = CreateClient(config.Endpoint, config.ModelId);
    }

    /// <summary>
    /// Obtém o cliente Ollama configurado para chat.
    /// </summary>
    /// <returns>Instância de <see cref="OllamaApiClient"/> para chat.</returns>
    /// <example>
    /// <code>
    /// var client = adapter.GetClientChat();
    /// </code>
    /// </example>
    public OllamaApiClient GetClientChat() => _clientChat;

    /// <summary>
    /// Obtém o cliente Ollama configurado para embeddings.
    /// </summary>
    /// <returns>Instância de <see cref="OllamaApiClient"/> para embeddings.</returns>
    /// <example>
    /// <code>
    /// var client = adapter.GetClientEmbedding();
    /// </code>
    /// </example>
    public OllamaApiClient GetClientEmbedding() => _clientEmbedding;

    /// <summary>
    /// Cria um cliente Ollama apontando para a URL e modelo informados.
    /// </summary>
    /// <param name="url">Endpoint do servidor Ollama.</param>
    /// <param name="modelId">Identificador do modelo.</param>
    /// <returns>Cliente configurado.</returns>
    private static OllamaApiClient CreateClient(string url, string modelId)
    {
        var uri = new Uri(url);
        var clientInstance = new OllamaApiClient(uri);
        clientInstance.SelectedModel = modelId;
        return clientInstance;
    }

    /// <summary>
    /// Gera chat completion via streaming Ollama e converte Markdown para HTML.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Resposta textual do modelo.</returns>
    /// <example>
    /// <code>
    /// string reply = await adapter.GenerateChatCompletionAsync(messages);
    /// </code>
    /// </example>
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

    /// <summary>
    /// Gera o embedding vetorial do texto via Ollama.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Array de floats do embedding.</returns>
    /// <example>
    /// <code>
    /// float[] emb = await adapter.GenerateEmbeddingAsync("praia");
    /// </code>
    /// </example>
    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.");

        var embedding = await _clientChat.EmbedAsync(text, CancellationToken.None);
        return embedding.Embeddings?.FirstOrDefault()?.ToArray() ?? Array.Empty<float>();
    }

    /// <summary>
    /// Converte o papel HotelWise para o papel esperado pela API Ollama.
    /// </summary>
    /// <param name="role">Descrição textual do papel.</param>
    /// <returns>Papel Ollama (<c>system</c>, <c>user</c> ou <c>assistant</c>).</returns>
    private static string ConvertRole(string role) =>
        role.ToLower() switch
        {
            "agent" => "system",
            "system" => "system",
            "user" => "user",
            "assistant" => "assistant",
            _ => "user"
        };

    /// <summary>
    /// Gera chat por agente; no Ollama delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Resposta textual do modelo.</returns>
    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);

    /// <summary>
    /// Gera chat por agente com RAG simples; no Ollama delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Resposta textual do modelo.</returns>
    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);
}
#endif
