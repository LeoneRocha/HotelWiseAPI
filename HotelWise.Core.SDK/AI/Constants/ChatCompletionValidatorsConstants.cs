namespace HotelWise.Core.SDK.AI.Constants;

/// <summary>
/// Limites de validação para chat completion e assistente conversacional.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ChatCompletionValidatorsConstants
{
    public const int MaximumMessages = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaximumMessages;

    public const int MaxTextLength = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTextLength;

    public const int MaximumLengthContent = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaximumLengthContent;

    public const int MaxTokensPerMessage = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessage;

    public const int MaxTextToken = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTextToken;

    public const int MaxTotalTokens = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTotalTokens;

    public const int MaxTokensPerMessageContext = SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;
}
