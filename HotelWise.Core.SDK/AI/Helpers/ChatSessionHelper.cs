#if NET8_0_OR_GREATER
using System.Text;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Helpers;

namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitários para montagem de contexto de sessão de chat a partir do histórico de prompts.
/// Concatena mensagens e remove HTML para uso em inferência ou logs.
/// </summary>
/// <example>
/// <code>
/// var history = new[]
/// {
///     new PromptMessageVO { RoleType = RoleAiPromptsType.User, Content = "Olá" },
///     new PromptMessageVO { RoleType = RoleAiPromptsType.Assistant, Content = "Oi!" }
/// };
/// string contexto = ChatSessionHelper.GetHistoryContext(history);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Helpers.ChatSessionHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ChatSessionHelper
{
    /// <summary>
    /// Concatena o histórico de mensagens e remove marcações HTML do resultado.
    /// </summary>
    /// <param name="history">Array de mensagens do histórico.</param>
    /// <returns>Texto de contexto sem HTML, ou string vazia se o histórico for nulo/vazio.</returns>
    /// <example>
    /// <code>
    /// string ctx = ChatSessionHelper.GetHistoryContext(mensagens);
    /// </code>
    /// </example>
    public static string GetHistoryContext(PromptMessageVO[] history)
    {
        if (history == null || history.Length == 0)
            return string.Empty;

        return HtmlHelper.RemoveHtml(GenerateContextMessage(history));
    }

    /// <summary>
    /// Gera o texto de contexto concatenando papel e conteúdo de cada mensagem.
    /// </summary>
    /// <param name="history">Array de mensagens do histórico.</param>
    /// <returns>Texto multilinha no formato <c>Role: Content</c>.</returns>
    /// <example>
    /// <code>
    /// string raw = ChatSessionHelper.GenerateContextMessage(mensagens);
    /// </code>
    /// </example>
    public static string GenerateContextMessage(PromptMessageVO[] history)
    {
        var contextBuilder = new StringBuilder();

        foreach (var message in history.Where(m => !string.IsNullOrEmpty(m.Content)))
        {
            contextBuilder.AppendLine($"{message.RoleType}: {message.Content}");
        }

        return contextBuilder.ToString().Trim();
    }
}
#endif
