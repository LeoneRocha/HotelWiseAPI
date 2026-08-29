namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.MarkdownHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class MarkdownHelper
    {
        public static string RemoveMarkdown(string markdownText) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.RemoveMarkdown(markdownText);

        public static bool HasMarkdown(string text) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.HasMarkdown(text);

        public static string ConvertToHtmlIfMarkdown(string markdownText) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.ConvertToHtmlIfMarkdown(markdownText);
    }
}
