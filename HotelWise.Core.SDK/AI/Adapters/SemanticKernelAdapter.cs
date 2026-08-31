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
/// Implementa <see cref="IAIInferenceAdapter"/> com chat completion, agentes
/// (<see cref="ChatCompletionAgent"/>) e embeddings, incluindo fluxo RAG simples
/// que injeta mensagens de contexto no histórico.
/// </summary>
/// <example>
/// <code>
/// var adapter = new SemanticKernelAdapter(appConfig, serviceProvider);
/// string reply = await adapter.GenerateChatCompletionAsync(messages);
/// float[] emb = await adapter.GenerateEmbeddingAsync("texto");
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.SemanticKernelAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SemanticKernelAdapter : IAIInferenceAdapter
{
    /// <summary>
    /// Kernel do Semantic Kernel.
    /// </summary>
    private readonly Kernel _kernel;

    /// <summary>
    /// Serviço de chat completion obtido do kernel.
    /// </summary>
    private readonly IChatCompletionService _chatCompletionService;

    /// <summary>
    /// Gerador de embeddings obtido do kernel.
    /// </summary>
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

    /// <summary>
    /// Separador visual entre fragmentos de contexto RAG.
    /// </summary>
    private const string LineSeparator = "--------------------------------";

    /// <summary>
    /// Inicializa o adapter resolvendo <see cref="Kernel"/>, chat e embeddings do DI.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA (reservada para extensões).</param>
    /// <param name="serviceProvider">Provedor de serviços com Kernel registrado.</param>
    public SemanticKernelAdapter(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
    {
        _kernel = serviceProvider.GetRequiredService<Kernel>()
            ?? throw new InvalidOperationException("Kernel não foi inicializado corretamente.");
        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        _embeddingGenerator = _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
    }

    /// <summary>
    /// Gera chat completion a partir do histórico de mensagens system/user/assistant.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Resposta do modelo, convertida para HTML se for Markdown.</returns>
    /// <example>
    /// <code>
    /// string html = await adapter.GenerateChatCompletionAsync(messages);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        var resultInference = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference?.Content);
    }

    /// <summary>
    /// Gera chat completion utilizando agente configurado pela mensagem com role Agent.
    /// </summary>
    /// <param name="messages">Histórico incluindo mensagem Agent.</param>
    /// <returns>Resposta do agente, convertida para HTML se for Markdown.</returns>
    /// <example>
    /// <code>
    /// string reply = await adapter.GenerateChatCompletionByAgentAsync(messages);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));
        var resultInference = await ProcessAgentResultAsync(agent, chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference);
    }

    /// <summary>
    /// Gera chat completion por agente com contexto RAG simples (role Context).
    /// </summary>
    /// <param name="messages">Histórico incluindo Agent e Context com DataContextRag.</param>
    /// <returns>Resposta enriquecida pelo contexto, convertida para HTML se for Markdown.</returns>
    /// <example>
    /// <code>
    /// string reply = await adapter.GenerateChatCompletionByAgentSimpleRagAsync(messages);
    /// </code>
    /// </example>
    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages)
    {
        ValidateMessages(messages);
        var chatHistory = BuildChatHistory(messages);
        AddContextToChatHistory(chatHistory, messages);
        var agent = BuildAgent(messages.First(m => m.RoleType == RoleAiPromptsType.Agent));
        var resultInference = await ProcessAgentResultAsync(agent, chatHistory);
        return ProcessResultContentToHtmlIfMarkdown(resultInference);
    }

    /// <summary>
    /// Gera o embedding vetorial do texto informado.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Array de floats do embedding.</returns>
    /// <example>
    /// <code>
    /// float[] emb = await adapter.GenerateEmbeddingAsync("hotel à beira-mar");
    /// </code>
    /// </example>
    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or empty.");

        var embedding = await _embeddingGenerator.GenerateAsync(text);
        if (embedding.Vector.Length == 0)
            return Array.Empty<float>();
        return embedding.Vector.ToArray();
    }

    /// <summary>
    /// Valida se o array de mensagens não é nulo ou vazio.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    private static void ValidateMessages(PromptMessageVO[] messages)
    {
        if (messages == null || messages.Length == 0)
            throw new ArgumentException("Messages cannot be null or empty.");
    }

    /// <summary>
    /// Converte o conteúdo para HTML quando detectar Markdown.
    /// </summary>
    /// <param name="content">Conteúdo retornado pelo modelo.</param>
    /// <returns>Conteúdo processado.</returns>
    private static string ProcessResultContentToHtmlIfMarkdown(string? content) =>
        MarkdownHelper.ConvertToHtmlIfMarkdown(content ?? string.Empty);

    /// <summary>
    /// Invoca o agente e agrega o conteúdo das mensagens retornadas.
    /// </summary>
    /// <param name="agent">Agente de chat completion.</param>
    /// <param name="chatHistory">Histórico de conversa.</param>
    /// <returns>Texto concatenado da resposta do agente.</returns>
    private static async Task<string> ProcessAgentResultAsync(ChatCompletionAgent agent, ChatHistory chatHistory)
    {
        var resultBuilder = new StringBuilder();
        await foreach (var message in agent.InvokeAsync(chatHistory))
        {
            resultBuilder.Append(message.Message.Content);
        }
        return resultBuilder.ToString();
    }

    /// <summary>
    /// Monta o <see cref="ChatHistory"/> a partir das mensagens system/user/assistant.
    /// </summary>
    /// <param name="messages">Histórico de prompts.</param>
    /// <returns>Histórico de chat do Semantic Kernel.</returns>
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

    /// <summary>
    /// Constrói o agente a partir da mensagem com role Agent.
    /// </summary>
    /// <param name="agentMessage">Mensagem contendo instruções e nome do agente.</param>
    /// <returns>Instância de <see cref="ChatCompletionAgent"/>.</returns>
    private ChatCompletionAgent BuildAgent(PromptMessageVO agentMessage) =>
        new ChatCompletionAgent
        {
            Instructions = agentMessage.Content,
            Name = agentMessage.AgentName,
            Kernel = _kernel
        };

    /// <summary>
    /// Injeta no histórico o contexto RAG proveniente de mensagens com role Context.
    /// </summary>
    /// <param name="chatHistory">Histórico de chat a enriquecer.</param>
    /// <param name="messages">Mensagens de origem, incluindo Context.</param>
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
