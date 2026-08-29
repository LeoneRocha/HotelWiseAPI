namespace HotelWise.Domain.Helpers.AI
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.HtmlHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class HtmlHelper
    {
        public static string RemoveHtml(string html) =>
            HotelWise.Core.SDK.Helpers.HtmlHelper.RemoveHtml(html);
    }
}
