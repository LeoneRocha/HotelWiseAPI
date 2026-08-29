#if NET8_0_OR_GREATER
using System.Text.RegularExpressions;
using Markdig;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de Markdown.
/// </summary>
public static class MarkdownHelper
{
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

    public static bool HasMarkdown(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var markdownPattern = @"(\*\*|__|~~|`|_|[*[\]()!#>\-])";
        return Regex.IsMatch(text, markdownPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }

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
