#if NET8_0_OR_GREATER
using HtmlAgilityPack;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de manipulação de HTML, baseados em HtmlAgilityPack,
/// para extrair texto plano a partir de markup.
/// </summary>
/// <example>
/// <code>
/// var plain = HtmlHelper.RemoveHtml("&lt;p&gt;Olá&lt;/p&gt;");
/// // plain == "Olá"
/// </code>
/// </example>
public static class HtmlHelper
{
    /// <summary>
    /// Remove todas as tags HTML do conteúdo, retornando o texto puro.
    /// </summary>
    /// <param name="html">String contendo tags HTML.</param>
    /// <returns>Texto sem formatação HTML.</returns>
    public static string RemoveHtml(string html) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.HtmlHelper.RemoveHtml(html);
}
#endif
