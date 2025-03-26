using HotelWise.Domain.Dto.SemanticKernel;
using System.Text.RegularExpressions;

namespace HotelWise.Service.Bussines
{
    public static class HotelResponseProcessor
    {
        public static List<HotelInfo> ProcessResponse(string markdownResponse)
        {
            // Lista para armazenar os dados dos hotéis
            List<HotelInfo> hotelInfos = new List<HotelInfo>();

            // Regex para encontrar os IDs ocultos nos comentários HTML
            string idPattern = @"<!--\s*ID-Hotel:\s*(\d+)\s*-->";
            MatchCollection matches = Regex.Matches(markdownResponse, idPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

            foreach (Match match in matches)
            {
                // Tenta converter o ID do hotel para o tipo long
                if (long.TryParse(match.Groups[1].Value, out long hotelId))
                {
                    // Adiciona à lista com as informações do hotel
                    hotelInfos.Add(new HotelInfo
                    {
                        Id = hotelId,
                        IdType = "Hotel",
                        LogMessage = $"Hotel ID encontrado: {hotelId}"
                    });
                }
                else
                {
                    // Gera um log caso a conversão falhe
                    Console.WriteLine($"Falha ao converter o ID do hotel: {match.Groups[1].Value}");
                }
            }

            return hotelInfos;
        }
    }
}