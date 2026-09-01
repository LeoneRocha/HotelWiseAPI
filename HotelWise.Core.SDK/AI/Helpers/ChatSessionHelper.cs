#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.DTO;
using SchHelpers = SmartCoreHub.Core.SDK.Service.AI.Helpers;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitários para montagem de contexto de sessão de chat a partir do histórico de prompts.
/// </summary>
public static class ChatSessionHelper
{
    /// <summary>
    /// Obtém o contexto textual formatado a partir do histórico de prompts.
    /// </summary>
    /// <param name="history">Histórico de mensagens de prompt.</param>
    /// <returns>String com o contexto montado.</returns>
    public static string GetHistoryContext(PromptMessageVO[] history) =>
        SchHelpers.ChatSessionHelper.GetHistoryContext(history);

    /// <summary>
    /// Gera a mensagem de contexto formatada a partir do histórico de prompts.
    /// </summary>
    /// <param name="history">Histórico de mensagens de prompt.</param>
    /// <returns>String formatada com a mensagem de contexto.</returns>
    public static string GenerateContextMessage(PromptMessageVO[] history) =>
        SchHelpers.ChatSessionHelper.GenerateContextMessage(history);
}
#endif
