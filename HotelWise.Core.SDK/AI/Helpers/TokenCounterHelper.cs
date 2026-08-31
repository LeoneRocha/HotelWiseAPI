using HotelWise.Core.SDK.AI.DTO;
using SchHelpers = SmartCoreHub.Core.SDK.Service.AI.Helpers;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Contagem aproximada de tokens para prompts e contextos RAG.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Helpers.TokenCounterHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class TokenCounterHelper
{
    public static int CountTokens(string text) =>
        SchHelpers.TokenCounterHelper.CountTokens(text);

    public static int CalculateDataVectorLength(DataVectorVO[] dataContextRag) =>
        SchHelpers.TokenCounterHelper.CalculateDataVectorLength(dataContextRag);

    public static int CalculateTotalDataVectorLength(PromptMessageVO[] promptMessages) =>
        SchHelpers.TokenCounterHelper.CalculateTotalDataVectorLength(promptMessages);

    public static int CalculateTotalTokens(PromptMessageVO[] promptMessages) =>
        SchHelpers.TokenCounterHelper.CalculateTotalTokens(promptMessages);

    public static int CountTokensFromPrompt(PromptMessageVO promptMessage) =>
        SchHelpers.TokenCounterHelper.CountTokensFromPrompt(promptMessage);
}
