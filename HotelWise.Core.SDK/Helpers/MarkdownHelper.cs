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
public static class MarkdownHelper
{
    /// <summary>Remove formatações Markdown do texto informado.</summary>
    /// <param name="markdownText">Texto com formatação Markdown.</param>
    /// <returns>Texto sem sintaxe Markdown.</returns>
    public static string RemoveMarkdown(string markdownText) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.RemoveMarkdown(markdownText);

    /// <summary>Verifica se o texto contém sintaxe ou marcações Markdown.</summary>
    /// <param name="text">Texto de entrada.</param>
    /// <returns>True se contém Markdown; caso contrário false.</returns>
    public static bool HasMarkdown(string text) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.HasMarkdown(text);

    /// <summary>Converte o texto para HTML caso contenha sintaxe Markdown.</summary>
    /// <param name="markdownText">Texto de entrada.</param>
    /// <returns>HTML gerado ou o próprio texto original.</returns>
    public static string ConvertToHtmlIfMarkdown(string markdownText) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper.ConvertToHtmlIfMarkdown(markdownText);
}
#endif
