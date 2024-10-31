using Bogus;
using HotelWise.Domain.Model;
using Newtonsoft.Json;
using System.Text;

namespace HotelWise.Data.Context.Configure.Mock
{
    public static class HotelsMockData
    {
        public static Hotel[] GetHotels()
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();

            hotels.Add(new Hotel
            {
                HotelId = 1,
                HotelName = "Hotel Example",
                Description = "An example hotel",
                Tags = new[] { "Luxury", "Spa" },
                Stars = 5,
                InitialRoomPrice = 200.00m,
                ZipCode = "12345",
                Location = "Example City"
            });

            GetHotelsGenerateFromBogus(faker, hotels);

            return hotels.ToArray();
        }
        public static async Task<Hotel[]> GetHotelsAsync()
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();
              
            await GetHotelsGenerateAi(faker, hotels);

            return hotels.ToArray();
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

        private static async Task GetHotelsGenerateAi(Faker faker, List<Hotel> hotels)
        {
            for (int i = 2; i <= 1000; i++)
            {
                var prompt = $"Gere uma descrição e tags para um hotel chamado {faker.Company.CompanyName()} localizado em {faker.Address.City()} com {faker.Random.Int(1, 5)} estrelas.";
                var descriptionAndTags = await GenerateDescriptionAndTags(prompt);
                var splitResult = descriptionAndTags.Split('|');
                var description = splitResult[0].Trim();
                var tags = splitResult[1].Trim().Split(',');

                hotels.Add(new Hotel
                {
                    HotelId = i,
                    HotelName = faker.Company.CompanyName(),
                    Description = description,
                    Tags = tags,
                    Stars = (byte)faker.Random.Int(1, 5),
                    InitialRoomPrice = faker.Random.Decimal(100, 1000),
                    ZipCode = faker.Address.ZipCode(),
                    Location = faker.Address.City()
                });
            }
        }

        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> GenerateDescriptionAndTags(string prompt)
        {
            //https://github.com/jgravelle/GroqApiLibrary
            var apiKey = "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";
            var requestBody = new
            {
                prompt = prompt,
                max_tokens = 100
            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/engines/davinci-codex/completions", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

            return openAIResponse!.Choices[0].Text;
        }
    }
}
