﻿using Markdig;
using System.Text.RegularExpressions;
namespace HotelWise.Domain.Helpers
{
    public static class MarkdownHelper
    {
        /// <summary>
        /// Remove a formatação de Markdown de um texto.
        /// </summary>
        /// <param name="markdownText">O texto com formatação Markdown.</param>
        /// <returns>O texto limpo, sem formatação Markdown.</returns>
        public static string RemoveMarkdown(string markdownText)
        {
            if (string.IsNullOrWhiteSpace(markdownText))
                return string.Empty;

            // Expressões regulares para remover elementos Markdown
            var result = Regex.Replace(markdownText, @"(\*\*|__|~~|`|_|[*[\]()])", "", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Negrito, itálico, links, etc.
            result = Regex.Replace(result, @"\!\[.*?\]\(.*?\)", "", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Imagens ![alt](url)
            result = Regex.Replace(result, @"\[.*?\]\(.*?\)", "", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Links [text](url)
            result = Regex.Replace(result, @"#{1,6}\s*", "", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Headers (#, ##, ###, etc.)
            result = Regex.Replace(result, @">\s*", "", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Blocos de citação >
            result = Regex.Replace(result, @"^\s*\-\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100)); // Listas com '-'
            result = Regex.Replace(result, @"^\s*\*\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100)); // Listas com '*'
            result = Regex.Replace(result, @"^\s*\d+\.\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100)); // Listas ordenadas '1. ...'
            return result.Trim();
        }

        /// <summary>
        /// Verifica se um texto contém Markdown.
        /// </summary>
        /// <param name="text">O texto a ser verificado.</param>
        /// <returns>Verdadeiro se o texto contiver elementos Markdown; caso contrário, falso.</returns>
        public static bool HasMarkdown(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            // Expressão regular para detectar padrões comuns de Markdown
            var markdownPattern = @"(\*\*|__|~~|`|_|[*[\]()!#>\-])";
            return Regex.IsMatch(text, markdownPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
        }

        /// <summary>
        /// Verifica se o texto contém Markdown e o converte para HTML, se aplicável.
        /// </summary>
        /// <param name="markdownText">O texto potencialmente em Markdown.</param>
        /// <returns>O texto convertido em HTML ou o próprio texto se não contiver Markdown.</returns>
        public static string ConvertToHtmlIfMarkdown(string markdownText)
        {
            if (HasMarkdown(markdownText))
            {
                // Converte o Markdown para HTML
                return Markdown.ToHtml(markdownText);
            }
            // Retorna o texto original caso não seja Markdown
            return markdownText;
        }
    }
}