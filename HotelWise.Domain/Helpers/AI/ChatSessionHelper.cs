using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Helpers;
using System.Text;

namespace HotelWise.Domain.Helpers.AI
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (ChatSessionHistoryDto / PromptMessageVO Domain).
    /// Core expõe overload com PromptMessageVO[] em HotelWise.Core.SDK.AI.Helpers.ChatSessionHelper.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Helpers.ChatSessionHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public static class ChatSessionHelper
    {
        public static string GetHistoryContext(ChatSessionHistoryDto chatSession)
        {
            if (chatSession.PromptMessageHistory.Length == 0)
                return string.Empty;

            var contextBuilder = new StringBuilder();
            contextBuilder.AppendLine(GenerateContextMessage(chatSession.PromptMessageHistory));
            return HtmlHelper.RemoveHtml(contextBuilder.ToString().Trim());
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
}
