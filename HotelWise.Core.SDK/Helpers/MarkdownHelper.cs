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
    public static string RemoveMarkdown(string markdownText) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.RemoveMarkdown(markdownText);

    public static bool HasMarkdown(string text) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.HasMarkdown(text);

    public static string ConvertToHtmlIfMarkdown(string markdownText) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.ConvertToHtmlIfMarkdown(markdownText);
}

#endif
