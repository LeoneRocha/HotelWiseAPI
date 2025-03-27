using HtmlAgilityPack;
namespace HotelWise.Domain.Helpers.AI
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Remove o HTML do texto e retorna apenas o conteúdo de texto.
        /// </summary>
        /// <param name="html">Texto com HTML.</param>
        /// <returns>Texto limpo.</returns>
        public static string RemoveHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Extrai apenas o texto limpo
            return htmlDocument.DocumentNode.InnerText;
        }
    }
}