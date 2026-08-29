namespace HotelWise.Domain.Constants.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public static class ChatCompletionValidatorsConstants
    {
        public const int MaximumMessages = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaximumMessages;
        public const int MaxTextLength = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaxTextLength;
        public const int MaximumLengthContent = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaximumLengthContent;
        public const int MaxTokensPerMessage = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessage;
        public const int MaxTextToken = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaxTextToken;
        public const int MaxTotalTokens = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaxTotalTokens;
        public const int MaxTokensPerMessageContext = HotelWise.Core.SDK.AI.Constants.ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;
    }
}
