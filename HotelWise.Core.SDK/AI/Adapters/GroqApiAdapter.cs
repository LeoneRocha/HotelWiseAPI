#if NET8_0_OR_GREATER
using System.Text.Json.Nodes;
using GroqApiLibrary;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Groq API.
/// Implementa <see cref="IAIInferenceAdapter"/> para chat completion JSON;
/// embeddings não estão implementados neste adapter.
/// </summary>
/// <example>
/// <code>
/// var adapter = new GroqApiAdapter(appConfig);
/// string reply = await adapter.GenerateChatCompletionAsync(messages);
/// </code>
/// </example>
public class GroqApiAdapter : IAIInferenceAdapter
{
    /// <summary>
    /// Cliente da biblioteca Groq.
    /// </summary>
    private readonly GroqApiClient _groqApiClient;

    /// <summary>
    /// Identificador do modelo de chat configurado.
    /// </summary>
    private readonly string _model;

    /// <summary>
    /// Inicializa o cliente Groq com chave e modelo de <see cref="IApplicationIAConfig.GroqApiConfig"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA.</param>
    public GroqApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _groqApiClient = new GroqApiClient(applicationConfig.GroqApiConfig.ApiKey);
        _model = applicationConfig.GroqApiConfig.ModelId;
    }

    /// <summary>
    /// Gera chat completion enviando mensagens em formato JSON à Groq API.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo da primeira choice, ou string vazia.</returns>
    /// <example>
    /// <code>
    /// string reply = await adapter.GenerateChatCompletionAsync(messages);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        var request = new JsonObject
        {
            ["model"] = _model,
            ["messages"] = new JsonArray(messages.Select(m => new JsonObject
            {
                ["role"] = GetRole(m.RoleType),
                ["content"] = m.Content
            }).ToArray())
        };

        var result = await _groqApiClient.CreateChatCompletionAsync(request);
        var resultOut = result?["choices"]?[0]?["message"]?["content"]?.ToString();
        return resultOut ?? string.Empty;
    }

    /// <summary>
    /// Mapeia o papel HotelWise para a string de role da Groq API.
    /// </summary>
    /// <param name="roleType">Tipo de papel da mensagem.</param>
    /// <returns>Role textual (<c>system</c>, <c>user</c> ou <c>assistant</c>).</returns>
    private static string GetRole(RoleAiPromptsType roleType) =>
        roleType switch
        {
            RoleAiPromptsType.System => "system",
            RoleAiPromptsType.Agent => "system",
            RoleAiPromptsType.User => "user",
            RoleAiPromptsType.Assistant => "assistant",
            _ => "user"
        };

    /// <summary>
    /// Gera chat por agente; na Groq delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);

    /// <summary>
    /// Embeddings não são suportados neste adapter.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Não retorna; sempre lança <see cref="NotImplementedException"/>.</returns>
    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        throw new NotImplementedException();

    /// <summary>
    /// Gera chat por agente com RAG simples; na Groq delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);
}
#endif
