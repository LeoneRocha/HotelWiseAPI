#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Orquestra chamadas de inferência via fábrica de adapters.
/// Implementa <see cref="IAIInferenceService"/> delegando chat e embeddings
/// ao <see cref="IAIInferenceAdapter"/> correspondente ao tipo solicitado.
/// </summary>
/// <example>
/// <code>
/// // Registro DI
/// services.AddScoped&lt;IAIInferenceService, AIInferenceService&gt;();
///
/// // Uso
/// string reply = await service.GenerateChatCompletionAsync(
///     messages, InferenceAiAdapterType.SemanticKernel);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AIInferenceService : IAIInferenceService
{
    /// <summary>
    /// Fábrica de adapters de inferência.
    /// </summary>
    private readonly IAIInferenceAdapterFactory _adapterFactory;

    /// <summary>
    /// Inicializa o serviço com a fábrica de adapters.
    /// </summary>
    /// <param name="configuration">Configuração da aplicação (reservada para extensões).</param>
    /// <param name="adapterFactory">Fábrica de <see cref="IAIInferenceAdapter"/>.</param>
    public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
    {
        _adapterFactory = adapterFactory;
    }

    /// <summary>
    /// Gera chat completion via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta.</returns>
    /// <example>
    /// <code>
    /// var reply = await service.GenerateChatCompletionAsync(msgs, InferenceAiAdapterType.GroqApi);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionAsync(messages);
    }

    /// <summary>
    /// Gera chat completion por agente via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico incluindo configuração do agente.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta do agente.</returns>
    /// <example>
    /// <code>
    /// var reply = await service.GenerateChatCompletionByAgentAsync(msgs, InferenceAiAdapterType.SemanticKernel);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionByAgentAsync(messages);
    }

    /// <summary>
    /// Gera chat completion por agente com RAG simples via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico incluindo agente e contexto RAG.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta enriquecida.</returns>
    /// <example>
    /// <code>
    /// var reply = await service.GenerateChatCompletionByAgentSimpleRagAsync(msgs, InferenceAiAdapterType.SemanticKernel);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateChatCompletionByAgentSimpleRagAsync(messages);
    }

    /// <summary>
    /// Gera embedding via o adapter informado.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Array de floats do embedding.</returns>
    /// <example>
    /// <code>
    /// float[] emb = await service.GenerateEmbeddingAsync("texto", InferenceAiAdapterType.Ollama);
    /// </code>
    /// </example>
    public async Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType)
    {
        var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
        return await adapter.GenerateEmbeddingAsync(text);
    }
}
#endif
