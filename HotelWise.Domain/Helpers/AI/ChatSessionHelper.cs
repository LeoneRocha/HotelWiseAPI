using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.IA;
using System.Text;

namespace HotelWise.Domain.Helpers.AI
{
    public static class ChatSessionHelper
    {
        /// <summary>
        /// Recupera o contexto de histórico concatenando todos os conteúdos das mensagens.
        /// </summary>
        /// <param name="chatSession">Instância de ChatSessionHistoryDto.</param>
        /// <returns>Uma string contendo todos os conteúdos das mensagens concatenados.</returns>
        public static string GetHistoryContext(ChatSessionHistoryDto chatSession)
        {
            if (chatSession.PromptMessageHistory.Length == 0 )
                return string.Empty;

            var contextBuilder = new StringBuilder();

            contextBuilder.AppendLine(GenerateContextMessage(chatSession.PromptMessageHistory));
            var result = HtmlHelper.RemoveHtml(contextBuilder.ToString().Trim());
            return result; // Remove espaços e linhas extras
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