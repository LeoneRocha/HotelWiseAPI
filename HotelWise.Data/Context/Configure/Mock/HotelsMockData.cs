using Bogus;
using GroqApiLibrary;
using HotelWise.Domain.Model;
using System.Text.Json.Nodes;

namespace HotelWise.Data.Context.Configure.Mock
{
    public static class HotelsMockData
    {
        public static Hotel[] GetHotels()
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();

            var hotelAddress = faker.Address;

            hotels.Add(new Hotel
            {
                HotelId = 1,
                HotelName = "Hotel Example",
                Description = "An example hotel",
                Tags = new[] { "Luxury", "Spa" },
                Stars = (byte)faker.Random.Int(1, 5),
                InitialRoomPrice = faker.Random.Decimal(100, 1000),
                ZipCode = hotelAddress.ZipCode(),
                Location = $"{hotelAddress.StreetSuffix()} {hotelAddress.StreetAddress()}",
                City = hotelAddress.City(),
                StateCode = hotelAddress.StateAbbr(),
            });

            return hotels.ToArray();
        }
        public static async Task<Hotel[]> GetHotelsAsync(int numberGerate)
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();
            await GetHotelsGenerateAi(faker, hotels, numberGerate);

            return hotels.ToArray();
        }

        private static async Task GetHotelsGenerateAi(Faker faker, List<Hotel> hotels, int numberGerate)
        {
            for (int i = 1; i <= numberGerate; i++)
            {
                var hotelAddress = faker.Address;
                var cityAdd = faker.Address.City();

                var prompt = $"Gere um nome de hotel, somente 1 hotel, uma descrição e de 5 a 10 tags para um hotel localizado em {cityAdd} com {faker.Random.Int(1, 5)} estrelas. Separe por |. Exemplo: Hotel Nome|Descriçao|tag 1|tag 2. A descrição nao pode passar de 500 caracteres.";

                var descriptionAndTags = await GenerateDescriptionAndTags(prompt);

                var splitResult = descriptionAndTags.Split('|');

                if (splitResult.Length < 3)
                {
                    Console.WriteLine($"Formato inesperado na resposta: {descriptionAndTags}");
                    continue;
                }

                var hotelName = splitResult[0].Trim();
                var description = splitResult[1].Trim();
                var tags = splitResult.Skip(2).Select(tag => tag.Trim()).ToArray();

                // Verificação adicional para tratar tags separadas por vírgulas
                if (tags.Length == 1 && tags[0].Contains(','))
                {
                    tags = tags[0].Split(',').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                }
                else
                {
                    tags = tags.Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                }

                // Validação das tags para não exceder 500 caracteres
                tags = ValidateAndAdjustTags(tags, 500);

                // Verificação adicional para garantir que os campos não estejam vazios
                if (string.IsNullOrEmpty(hotelName) || string.IsNullOrEmpty(description) || tags.Length == 0)
                {
                    Console.WriteLine($"Dados insuficientes na resposta: {descriptionAndTags}");
                    continue;
                }

                
                var hotelAdd = new Hotel
                {
                    HotelName = hotelName,
                    Description = description,
                    Tags = tags,
                    Stars = (byte)faker.Random.Int(1, 5),
                    InitialRoomPrice = faker.Random.Decimal(100, 1000),
                    ZipCode = hotelAddress.ZipCode(),
                    Location = $"{hotelAddress.StreetSuffix()} {hotelAddress.StreetAddress()}",
                    City = cityAdd,
                    StateCode = hotelAddress.StateAbbr(),
                };
                hotels.Add(hotelAdd);
            }
        }

        private static string[] ValidateAndAdjustTags(string[] tags, int maxLength)
        {
            var validTags = new List<string>();
            int currentLength = 0;

            foreach (var tag in tags)
            {
                if (currentLength + tag.Length + 1 > maxLength) // +1 para considerar a vírgula ou espaço
                {
                    break;
                }
                validTags.Add(tag);
                currentLength += tag.Length + 1; // +1 para considerar a vírgula ou espaço
            }

            return validTags.ToArray();
        }

        private static void GetHotelsGenerateFromBogus(Faker faker, List<Hotel> hotels)
        {
            //await Task.Run(() =>
            //{
            for (int i = 2; i <= 10; i++)
            {
                hotels.Add(new Hotel
                {
                    HotelId = i,
                    HotelName = faker.Company.CompanyName(),
                    Description = faker.Lorem.Sentence(),
                    Tags = faker.Lorem.Words(3).ToArray(),
                    Stars = (byte)faker.Random.Int(1, 5),
                    InitialRoomPrice = faker.Random.Decimal(100, 1000),
                    ZipCode = faker.Address.ZipCode(),
                    Location = faker.Address.City()
                });
            }
            //});
        }
        private static async Task<string> GenerateDescriptionAndTags(string prompt)
        {
            //https://github.com/jgravelle/GroqApiLibrary
            var apiKey = "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";

            var groqApi = new GroqApiClient(apiKey);

            var request = new JsonObject
            {
                ["model"] = "mixtral-8x7b-32768",
                ["messages"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["role"] = "user",
                        ["content"] =prompt
                    }
                }
            };

            var result = await groqApi.CreateChatCompletionAsync(request);
            var resultOut = result?["choices"]?[0]?["message"]?["content"]?.ToString();

            return resultOut ?? string.Empty;
        }
    }
}