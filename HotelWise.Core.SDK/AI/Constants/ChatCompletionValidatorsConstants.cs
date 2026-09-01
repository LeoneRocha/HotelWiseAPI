
namespace HotelWise.Core.SDK.AI.Constants;

/// <summary>
/// Limites de validação para chat completion e assistente conversacional.
/// </summary>
public static class ChatCompletionValidatorsConstants
{
    /// <summary>Número máximo de mensagens no histórico de conversa.</summary>
    public const int MaximumMessages = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaximumMessages;

    /// <summary>Comprimento máximo do texto da mensagem.</summary>
    public const int MaxTextLength = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTextLength;

    /// <summary>Comprimento máximo total do conteúdo de mensagens.</summary>
    public const int MaximumLengthContent = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaximumLengthContent;

    /// <summary>Quantidade máxima de tokens por mensagem individual.</summary>
    public const int MaxTokensPerMessage = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessage;

    /// <summary>Quantidade máxima de tokens de texto padrão.</summary>
    public const int MaxTextToken = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTextToken;

    /// <summary>Quantidade máxima total de tokens suportada no contexto.</summary>
    public const int MaxTotalTokens = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTotalTokens;

    /// <summary>Quantidade máxima de tokens por mensagem de contexto.</summary>
    public const int MaxTokensPerMessageContext = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;
}
