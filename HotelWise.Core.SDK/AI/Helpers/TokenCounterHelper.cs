using HotelWise.Core.SDK.AI.DTO;
using SchHelpers = SmartCoreHub.Core.SDK.Service.AI.Helpers;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Contagem aproximada de tokens para prompts e contextos RAG.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.AI.Helpers.TokenCounterHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.AI.Helpers.TokenCounterHelper em SmartCoreHub.Core.SDK.")]
public static class TokenCounterHelper
{
    /// <summary>Calcula a quantidade estimada de tokens para um texto.</summary>
    /// <param name="text">Texto de entrada.</param>
    /// <returns>Número estimado de tokens.</returns>
    public static int CountTokens(string text) =>
        SchHelpers.TokenCounterHelper.CountTokens(text);

    /// <summary>Calcula o comprimento total de tokens dos vetores de dados.</summary>
    /// <param name="dataContextRag">Contexto vetorial RAG.</param>
    /// <returns>Quantidade total estimada de tokens.</returns>
    public static int CalculateDataVectorLength(DataVectorVO[] dataContextRag) =>
        SchHelpers.TokenCounterHelper.CalculateDataVectorLength(dataContextRag);

    /// <summary>Calcula o comprimento total de dados vetoriais em uma lista de mensagens.</summary>
    /// <param name="promptMessages">Lista de mensagens de prompt.</param>
    /// <returns>Quantidade total estimada de tokens vetoriais.</returns>
    public static int CalculateTotalDataVectorLength(PromptMessageVO[] promptMessages) =>
        SchHelpers.TokenCounterHelper.CalculateTotalDataVectorLength(promptMessages);

    /// <summary>Calcula o total geral de tokens de uma lista de mensagens.</summary>
    /// <param name="promptMessages">Lista de mensagens de prompt.</param>
    /// <returns>Quantidade total estimada de tokens.</returns>
    public static int CalculateTotalTokens(PromptMessageVO[] promptMessages) =>
        SchHelpers.TokenCounterHelper.CalculateTotalTokens(promptMessages);

    /// <summary>Calcula a quantidade estimada de tokens de uma mensagem individual de prompt.</summary>
    /// <param name="promptMessage">Mensagem de prompt.</param>
    /// <returns>Quantidade estimada de tokens.</returns>
    public static int CountTokensFromPrompt(PromptMessageVO promptMessage) =>
        SchHelpers.TokenCounterHelper.CountTokensFromPrompt(promptMessage);
}
