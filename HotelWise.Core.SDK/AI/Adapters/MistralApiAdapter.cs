#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using Mistral.SDK;
using Mistral.SDK.DTOs;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Mistral API.
/// Implementa <see cref="IAIInferenceAdapter"/> com chat completion (modelo Medium)
/// e embeddings (Mistral Embed), mapeando roles do pipeline HotelWise.
/// </summary>
/// <example>
/// <code>
/// var adapter = new MistralApiAdapter(appConfig);
/// string reply = await adapter.GenerateChatCompletionAsync(messages);
/// float[] emb = await adapter.GenerateEmbeddingAsync("texto");
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.MistralApiAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiAdapter : IAIInferenceAdapter
{
    /// <summary>
    /// Cliente oficial da Mistral API.
    /// </summary>
    private readonly MistralClient _client;

    /// <summary>
    /// Inicializa o cliente Mistral com a chave de <see cref="IApplicationIAConfig.MistralApiConfig"/>.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA.</param>
    public MistralApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _client = new MistralClient(applicationConfig.MistralApiConfig.ApiKey);
    }

    /// <summary>
    /// Gera chat completion via Mistral Medium com parâmetros fixos de sampling.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    /// <example>
    /// <code>
    /// string reply = await adapter.GenerateChatCompletionAsync(messages);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        var chatMessages = messages.Select(m => new ChatMessage(
            GetRole(m),
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

    /// <summary>
    /// Mapeia o papel da mensagem HotelWise para o enum de role da Mistral.
    /// </summary>
    /// <param name="pm">Mensagem de prompt.</param>
    /// <returns>Role da Mistral API.</returns>
    private static ChatMessage.RoleEnum GetRole(PromptMessageVO pm) =>
        pm.RoleType switch
        {
            RoleAiPromptsType.System or RoleAiPromptsType.Agent => ChatMessage.RoleEnum.System,
            RoleAiPromptsType.User => ChatMessage.RoleEnum.User,
            RoleAiPromptsType.Assistant => ChatMessage.RoleEnum.Assistant,
            _ => ChatMessage.RoleEnum.User
        };

    /// <summary>
    /// Gera chat por agente; na Mistral delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);

    /// <summary>
    /// Gera o embedding vetorial do texto via Mistral Embed.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Array de floats do embedding.</returns>
    /// <example>
    /// <code>
    /// float[] emb = await adapter.GenerateEmbeddingAsync("hotel");
    /// </code>
    /// </example>
    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var request = new EmbeddingRequest(ModelDefinitions.MistralEmbed, new List<string>() { text }, EmbeddingRequest.EncodingFormatEnum.Float);
        var response = await _client.Embeddings.GetEmbeddingsAsync(request);
        var resultEmbedding = new List<float>();
        response.Data.ForEach(x => resultEmbedding.AddRange(x.Embedding.Select(eb => (float)eb).ToList()));
        return resultEmbedding.ToArray();
    }

    /// <summary>
    /// Gera chat por agente com RAG simples; na Mistral delega para <see cref="GenerateChatCompletionAsync"/>.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);
}
#endif
