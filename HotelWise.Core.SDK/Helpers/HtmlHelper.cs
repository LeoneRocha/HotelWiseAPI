#if NET8_0_OR_GREATER
using HtmlAgilityPack;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de HTML.
/// </summary>
public static class HtmlHelper
{
    public static string RemoveHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        return htmlDocument.DocumentNode.InnerText;
    }
}
#endif
