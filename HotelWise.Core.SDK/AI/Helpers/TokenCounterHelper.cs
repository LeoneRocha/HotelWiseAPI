using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Contagem aproximada de tokens para prompts.
/// </summary>
public static class TokenCounterHelper
{
    public static int CountTokens(string text)
    {
        return text.Length / 4;
    }

    public static int CalculateDataVectorLength(DataVectorVO[] dataContextRag)
    {
        if (dataContextRag == null || dataContextRag.Length == 0)
            return 0;

        return dataContextRag.Where(dv => dv != null && !string.IsNullOrEmpty(dv.DataVector))
            .Sum(dv => dv.DataVector.Length);
    }

    public static int CalculateTotalDataVectorLength(PromptMessageVO[] promptMessages)
    {
        if (promptMessages == null || promptMessages.Length == 0)
            return 0;

        return promptMessages
            .Where(p => p != null)
            .Sum(p => CalculateDataVectorLength(p.DataContextRag));
    }

    public static int CalculateTotalTokens(PromptMessageVO[] promptMessages)
    {
        if (promptMessages == null || promptMessages.Length == 0)
            return 0;

        return promptMessages.Sum(CountTokensFromPrompt);
    }

    public static int CountTokensFromPrompt(PromptMessageVO promptMessage)
    {
        if (promptMessage == null)
            return 0;

        int contentTokens = CountTokens(promptMessage.Content);
        int dataVectorTokens = CalculateDataVectorLength(promptMessage.DataContextRag);
        return contentTokens + dataVectorTokens;
    }
}
