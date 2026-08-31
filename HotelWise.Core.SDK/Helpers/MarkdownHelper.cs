#if NET8_0_OR_GREATER
using System.Text.RegularExpressions;
using Markdig;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de Markdown: remoção de sintaxe, detecção de markup e conversão
/// para HTML via Markdig quando o texto contém formatação Markdown.
/// </summary>
/// <example>
/// <code>
/// if (MarkdownHelper.HasMarkdown(text))
///     html = MarkdownHelper.ConvertToHtmlIfMarkdown(text);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class MarkdownHelper
{
    /// <summary>
    /// Remove marcadores Markdown comuns e devolve texto aproximado em plain text.
    /// </summary>
    /// <param name="markdownText">Texto com possível sintaxe Markdown.</param>
    /// <returns>Texto sem marcadores Markdown, ou string vazia se a entrada for vazia.</returns>
    public static string RemoveMarkdown(string markdownText)
    {
        if (string.IsNullOrWhiteSpace(markdownText))
            return string.Empty;

        var result = Regex.Replace(markdownText, @"(\*\*|__|~~|`|_|[*[\]()])", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"\!\[.*?\]\(.*?\)", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"\[.*?\]\(.*?\)", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"#{1,6}\s*", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @">\s*", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"^\s*\-\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"^\s*\*\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100));
        result = Regex.Replace(result, @"^\s*\d+\.\s+", "", RegexOptions.Multiline, TimeSpan.FromMilliseconds(100));
        return result.Trim();
    }

    /// <summary>
    /// Indica se o texto contém caracteres típicos de sintaxe Markdown.
    /// </summary>
    /// <param name="text">Texto a analisar.</param>
    /// <returns><c>true</c> se houver indícios de Markdown; caso contrário, <c>false</c>.</returns>
    public static bool HasMarkdown(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var markdownPattern = @"(\*\*|__|~~|`|_|[*[\]()!#>\-])";
        return Regex.IsMatch(text, markdownPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }

    /// <summary>
    /// Converte o texto para HTML via Markdig quando detectar Markdown; caso contrário, devolve o original.
    /// </summary>
    /// <param name="markdownText">Texto de entrada (Markdown ou plain).</param>
    /// <returns>HTML convertido ou o próprio texto se não houver Markdown.</returns>
    public static string ConvertToHtmlIfMarkdown(string markdownText)
    {
        if (HasMarkdown(markdownText))
        {
            return Markdown.ToHtml(markdownText);
        }
        return markdownText;
    }
}
#endif
