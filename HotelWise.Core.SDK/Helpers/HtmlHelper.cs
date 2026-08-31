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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.HtmlHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class HtmlHelper
{
    /// <summary>
    /// Remove tags HTML e retorna apenas o texto interno do documento.
    /// </summary>
    /// <param name="html">Fragmento ou documento HTML; nulo/vazio resulta em string vazia.</param>
    /// <returns>Texto plano sem markup.</returns>
    public static string RemoveHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        return htmlDocument.DocumentNode.InnerText;
    }
}
#endif
