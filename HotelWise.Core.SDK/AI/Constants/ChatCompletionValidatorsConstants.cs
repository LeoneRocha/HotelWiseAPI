namespace HotelWise.Core.SDK.AI.Constants;

/// <summary>
/// Limites de validação para chat completion.
/// </summary>
public static class ChatCompletionValidatorsConstants
{
    public const int MaximumMessages = 10;
    public const int MaxTextLength = 2500;
    public const int MaximumLengthContent = MaxTextLength;
    public const int MaxTokensPerMessage = 1000;
    public const int MaxTextToken = 1000;
    public const int MaxTotalTokens = 150_000;
    public const int MaxTokensPerMessageContext = 150_000;
}
