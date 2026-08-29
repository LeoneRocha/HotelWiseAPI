using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de inferência LLM (chat e embeddings).
/// Implementações concretas (Groq, Mistral, Ollama, Semantic Kernel) encapsulam
/// a comunicação com cada provedor no pipeline de IA/RAG.
/// </summary>
public interface IAIInferenceAdapter
{
    /// <summary>
    /// Gera o embedding vetorial do texto informado.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Array de floats representando o embedding.</returns>
    Task<float[]> GenerateEmbeddingAsync(string text);

    /// <summary>
    /// Gera uma resposta de chat completion a partir do histórico de mensagens.
    /// </summary>
    /// <param name="messages">Histórico de prompts (system, user, assistant, etc.).</param>
    /// <returns>Conteúdo textual da resposta do modelo.</returns>
    Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages);

    /// <summary>
    /// Gera chat completion utilizando agente (instruções em mensagem com role Agent).
    /// </summary>
    /// <param name="messages">Histórico incluindo a mensagem de configuração do agente.</param>
    /// <returns>Conteúdo textual da resposta do agente.</returns>
    Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages);

    /// <summary>
    /// Gera chat completion por agente com contexto RAG simples (mensagens de Context).
    /// </summary>
    /// <param name="messages">Histórico incluindo agente e fragmentos de contexto vetorial.</param>
    /// <returns>Conteúdo textual da resposta enriquecida pelo contexto RAG.</returns>
    Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages);
}

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// Resolve a implementação de <see cref="IAIInferenceAdapter"/> conforme
/// <see cref="InferenceAiAdapterType"/>.
/// </summary>
public interface IAIInferenceAdapterFactory
{
    /// <summary>
    /// Cria o adapter de inferência correspondente ao tipo informado.
    /// </summary>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter (Groq, Mistral, Ollama, Semantic Kernel).</param>
    /// <returns>Instância de <see cref="IAIInferenceAdapter"/>.</returns>
    IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType);
}

/// <summary>
/// Serviço de orquestração de inferência.
/// Encapsula a seleção do adapter e expõe operações de chat e embedding
/// usadas pelos fluxos conversacionais e RAG da aplicação.
/// </summary>
public interface IAIInferenceService
{
    /// <summary>
    /// Gera chat completion via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta do modelo.</returns>
    Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);

    /// <summary>
    /// Gera chat completion por agente via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico incluindo configuração do agente.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta do agente.</returns>
    Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);

    /// <summary>
    /// Gera chat completion por agente com RAG simples via o adapter informado.
    /// </summary>
    /// <param name="messages">Histórico incluindo agente e contexto RAG.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Conteúdo textual da resposta enriquecida pelo contexto.</returns>
    Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);

    /// <summary>
    /// Gera embedding via o adapter informado.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter de inferência.</param>
    /// <returns>Array de floats do embedding.</returns>
    Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType);
}

/// <summary>
/// Serviço de assistente conversacional voltado ao usuário final.
/// Combina embedding, histórico e resposta tipada (<see cref="AskAssistantResponse"/>).
/// </summary>
public interface IAssistantService
{
    /// <summary>
    /// Gera o embedding do texto informado para uso no fluxo do assistente.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Embedding gerado, ou <c>null</c> se não for possível.</returns>
    Task<float[]?> GenerateEmbeddingAsync(string text);

    /// <summary>
    /// Envia uma solicitação ao assistente e retorna as respostas geradas.
    /// </summary>
    /// <param name="request">Solicitação contendo mensagem e token de sessão.</param>
    /// <returns>Array de respostas do assistente, ou <c>null</c> em falha.</returns>
    Task<AskAssistantResponse[]?> AskAssistant(AskAssistantRequest request);

    /// <summary>
    /// Define o identificador do usuário autenticado no contexto do serviço.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    void SetUserId(long id);
}
