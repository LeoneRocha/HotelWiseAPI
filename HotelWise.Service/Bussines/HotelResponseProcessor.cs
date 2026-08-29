using System.Text.RegularExpressions;
using HotelWise.Domain.Dto.IA.SemanticKernel;

namespace HotelWise.Service.Bussines;

/// <summary>
/// Processador de respostas em Markdown geradas por IA, extraindo referências ocultas de IDs de hotéis.
/// </summary>
public static class HotelResponseProcessor
{
    /// <summary>
    /// Extrai os identificadores de hotéis contidos nos comentários HTML (&lt;!-- ID-Hotel: {id} --&gt;) da resposta em Markdown.
    /// </summary>
    /// <param name="markdownResponse">Texto formatado em Markdown retornado pelo modelo de linguagem.</param>
    /// <returns>Array de instâncias <see cref="HotelInfo"/> contendo os IDs encontrados.</returns>
    public static HotelInfo[] ProcessResponse(string markdownResponse)
    {
        // Regex para encontrar os IDs ocultos nos comentários HTML
        string idPattern = @"<!--\s*ID-Hotel:\s*(\d+)\s*-->";
        MatchCollection matches = Regex.Matches(markdownResponse, idPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

        // Usa LINQ para simplificar o loop e processar os dados diretamente
        var hotelInfos = matches
            .Select(match => new
            {
                Match = match,
                HotelId = long.TryParse(match.Groups[1].Value, out var id) ? id : (long?)null
            })
            .Where(x => x.HotelId.HasValue) // Filtra os IDs válidos
            .Select(x => new HotelInfo
            {
                Id = x.HotelId.Value,
                IdType = "Hotel", 
            })
            .ToArray();

        // Log para IDs que falharam na conversão
        foreach (var invalidMatch in matches.Where(match => !long.TryParse(match.Groups[1].Value, out _)))
        {
            Console.WriteLine($"Falha ao converter o ID do hotel: {invalidMatch.Groups[1].Value}");
        }

        return hotelInfos;
    }
}