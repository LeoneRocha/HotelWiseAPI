using HotelWise.Domain.Dto.SemanticKernel;
using System.Text.RegularExpressions;

namespace HotelWise.Service.Bussines
{
    public static class HotelResponseProcessor
    {
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
                    LogMessage = $"Hotel ID encontrado: {x.HotelId.Value}"
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
}