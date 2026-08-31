namespace HotelWise.Core.SDK.AI.Constants;

/// <summary>
/// Limites de validação para chat completion e assistente conversacional.
/// Consumidos pelos validators FluentValidation em <c>AI/Validation</c>
/// para restringir quantidade de mensagens, tamanho de texto e tokens.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ChatCompletionValidatorsConstants
{
    /// <summary>
    /// Número máximo de mensagens permitidas no histórico de prompts.
    /// </summary>
    public const int MaximumMessages = 10;

    /// <summary>
    /// Comprimento máximo do texto de uma mensagem (caracteres).
    /// </summary>
    public const int MaxTextLength = 2500;

    /// <summary>
    /// Comprimento máximo do conteúdo (alias de <see cref="MaxTextLength"/>).
    /// </summary>
    public const int MaximumLengthContent = MaxTextLength;

    /// <summary>
    /// Número máximo de tokens por mensagem comum (user/assistant/system).
    /// </summary>
    public const int MaxTokensPerMessage = 1000;

    /// <summary>
    /// Comprimento máximo do token de sessão (caracteres).
    /// </summary>
    public const int MaxTextToken = 1000;

    /// <summary>
    /// Soma máxima de tokens permitida no histórico completo.
    /// </summary>
    public const int MaxTotalTokens = 150_000;

    /// <summary>
    /// Número máximo de tokens para mensagens de contexto RAG.
    /// </summary>
    public const int MaxTokensPerMessageContext = 150_000;
}
