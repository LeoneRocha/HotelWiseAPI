using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Contagem aproximada de tokens para prompts e contextos RAG.
/// Usa heurística simples (caracteres / 4) e soma comprimentos de fragmentos vetoriais.
/// Consumido por validators e por <see cref="PromptMessageVO.TokenCount"/>.
/// </summary>
/// <example>
/// <code>
/// int tokens = TokenCounterHelper.CountTokens("Olá, mundo!");
/// int total = TokenCounterHelper.CalculateTotalTokens(mensagens);
/// </code>
/// </example>
public static class TokenCounterHelper
{
    /// <summary>
    /// Estima a quantidade de tokens de um texto (comprimento / 4).
    /// </summary>
    /// <param name="text">Texto a analisar.</param>
    /// <returns>Estimativa de tokens.</returns>
    /// <example>
    /// <code>
    /// int n = TokenCounterHelper.CountTokens(mensagem.Content);
    /// </code>
    /// </example>
    public static int CountTokens(string text)
    {
        return text.Length / 4;
    }

    /// <summary>
    /// Soma o comprimento dos fragmentos <see cref="DataVectorVO.DataVector"/> no contexto RAG.
    /// </summary>
    /// <param name="dataContextRag">Array de fragmentos de contexto.</param>
    /// <returns>Soma dos comprimentos, ou 0 se vazio/nulo.</returns>
    /// <example>
    /// <code>
    /// int len = TokenCounterHelper.CalculateDataVectorLength(msg.DataContextRag);
    /// </code>
    /// </example>
    public static int CalculateDataVectorLength(DataVectorVO[] dataContextRag)
    {
        if (dataContextRag == null || dataContextRag.Length == 0)
            return 0;

        return dataContextRag.Where(dv => dv != null && !string.IsNullOrEmpty(dv.DataVector))
            .Sum(dv => dv.DataVector.Length);
    }

    /// <summary>
    /// Soma o comprimento dos fragmentos RAG de todas as mensagens do histórico.
    /// </summary>
    /// <param name="promptMessages">Histórico de prompts.</param>
    /// <returns>Soma total dos comprimentos dos contextos.</returns>
    /// <example>
    /// <code>
    /// int totalCtx = TokenCounterHelper.CalculateTotalDataVectorLength(historico);
    /// </code>
    /// </example>
    public static int CalculateTotalDataVectorLength(PromptMessageVO[] promptMessages)
    {
        if (promptMessages == null || promptMessages.Length == 0)
            return 0;

        return promptMessages
            .Where(p => p != null)
            .Sum(p => CalculateDataVectorLength(p.DataContextRag));
    }

    /// <summary>
    /// Calcula o total aproximado de tokens de todas as mensagens do histórico.
    /// </summary>
    /// <param name="promptMessages">Histórico de prompts.</param>
    /// <returns>Soma de tokens de conteúdo e contexto RAG.</returns>
    /// <example>
    /// <code>
    /// int total = TokenCounterHelper.CalculateTotalTokens(historico);
    /// </code>
    /// </example>
    public static int CalculateTotalTokens(PromptMessageVO[] promptMessages)
    {
        if (promptMessages == null || promptMessages.Length == 0)
            return 0;

        return promptMessages.Sum(CountTokensFromPrompt);
    }

    /// <summary>
    /// Conta tokens de uma mensagem (conteúdo + fragmentos de contexto RAG).
    /// </summary>
    /// <param name="promptMessage">Mensagem de prompt.</param>
    /// <returns>Estimativa de tokens da mensagem, ou 0 se nula.</returns>
    /// <example>
    /// <code>
    /// int t = TokenCounterHelper.CountTokensFromPrompt(mensagem);
    /// </code>
    /// </example>
    public static int CountTokensFromPrompt(PromptMessageVO promptMessage)
    {
        if (promptMessage == null)
            return 0;

        int contentTokens = CountTokens(promptMessage.Content);
        int dataVectorTokens = CalculateDataVectorLength(promptMessage.DataContextRag);
        return contentTokens + dataVectorTokens;
    }
}
