#if NET8_0_OR_GREATER
using System.Text;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Helpers;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitários para montagem de contexto de sessão de chat.
/// </summary>
public static class ChatSessionHelper
{
    /// <summary>
    /// Concatena o histórico e remove HTML.
    /// </summary>
    public static string GetHistoryContext(PromptMessageVO[] history)
    {
        if (history == null || history.Length == 0)
            return string.Empty;

        return HtmlHelper.RemoveHtml(GenerateContextMessage(history));
    }

    public static string GenerateContextMessage(PromptMessageVO[] history)
    {
        var contextBuilder = new StringBuilder();

        foreach (var message in history)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                contextBuilder.AppendLine($"{message.RoleType}: {message.Content}");
            }
        }

        return contextBuilder.ToString().Trim();
    }
}
#endif
