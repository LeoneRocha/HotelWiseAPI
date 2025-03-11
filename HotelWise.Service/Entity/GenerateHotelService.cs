using Bogus;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Model;
using System.Text.RegularExpressions;

namespace HotelWise.Service.Entity
{
    public class GenerateHotelService : IGenerateHotelService
    {
        private readonly IAIInferenceService _aIInferenceService;

        public GenerateHotelService(IAIInferenceService aIInferenceService)
        {
            _aIInferenceService = aIInferenceService;
        }
        public async Task<Hotel> GetHotelAsync()
        {
            var faker = new Faker("pt_BR");
            Hotel? hotelAdd = await GetHotelGenerateAi(faker);

            if (hotelAdd != null)
            {
                return hotelAdd;
            }
            return new Hotel();
        }
        public async Task<Hotel[]> GetHotelsAsync(int numberGerate)
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();
            await GetHotelsGenerateAi(faker, hotels, numberGerate);

            return hotels.ToArray();
        }

        private async Task GetHotelsGenerateAi(Faker faker, List<Hotel> hotels, int numberGerate)
        {
            for (int i = 1; i <= numberGerate; i++)
            {
                Hotel? hotelAdd = await GetHotelGenerateAi(faker);
                if (hotelAdd != null)
                {
                    hotels.Add(hotelAdd);
                }
            }
        }

        private async Task<Hotel?> GetHotelGenerateAi(Faker faker)
        {
            var hotelAddress = faker.Address;

            var prompt = $"Gere um nome de hotel, uma descrição, Nome da cidade, sigla do estado de 2 carateres, CEP (codigo postal brasileiro) e de 5 a 10 tags  com {faker.Random.Int(1, 5)} estrelas. Separe por |. Siga o Exemplo: Hotel Nome|Descriçao|Cidade|sigla estado|CEP|tag 1|tag 2. A descrição nao pode passar de 500 caracteres. O CEP e nome da cidade e sigla do estado devem ter coerencia. Responda apenas com o que foi solicitado.";

            var descriptionAndTags = await GenerateDescriptionAndTags(prompt);

            var splitResult = descriptionAndTags.Split('|');

            if (splitResult.Length < 3)
            {
                Console.WriteLine($"Formato inesperado na resposta: {descriptionAndTags}");
                return null;
            }

            var hotelName = splitResult[0].Trim();
            var description = splitResult[1].Trim();
            var cityName = splitResult[2].Trim();
            var stateCode = splitResult[3].Trim();
            var cepCode = splitResult[4].Trim();

            var tags = splitResult.Skip(5).Select(tag => tag.Trim()).ToArray();

            // Verificação adicional para tratar tags separadas por vírgulas
            if (tags.Length == 1 && tags[0].Contains(','))
            {
                tags = tags[0].Split(',').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
            }
            else
            {
                tags = tags.Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
            }
            // Processar tags para garantir que cada item não tenha mais de uma palavra
            tags = ProcessTags(tags);

            // Validação das tags para não exceder 500 caracteres
            tags = ValidateAndAdjustTags(tags, 500);

            // Verificação adicional para garantir que os campos não estejam vazios
            if (string.IsNullOrEmpty(hotelName) || string.IsNullOrEmpty(description) || tags.Length == 0)
            {
                Console.WriteLine($"Dados insuficientes na resposta: {descriptionAndTags}");
                return null;
            }

            var hotelAdd = new Hotel
            {
                HotelName = hotelName,
                Description = description,
                Tags = tags,
                Stars = (byte)faker.Random.Int(1, 5),
                InitialRoomPrice = Math.Round(faker.Random.Decimal(100, 1000), 2, MidpointRounding.ToEven),
                City = cityName,
                StateCode = stateCode,
                ZipCode = cepCode,
                Location = $"{hotelAddress.StreetSuffix()} {hotelAddress.StreetAddress()}",
            };
            return hotelAdd;
        }

        private async Task<string> GenerateDescriptionAndTags(string prompt)
        {
            PromptMessageVO[] messages = [new PromptMessageVO() { RoleType = Domain.Enuns.RoleAiPromptsType.User, Content = prompt }];

            return await _aIInferenceService.GenerateChatCompletionAsync(messages, Domain.Enuns.IAInferenceAdapterType.GroqApi);
        }

        public static string[] ProcessTags(string[] tags)
        {
            var processedTags = new List<string>();
            var regex = new Regex(@"\d", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Expressão regular para detectar números
            var pattern = new Regex(@"^[a-zA-Z]+(_[a-zA-Z]+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(100)); // Expressão regular para detectar o padrão sigla_valida

            foreach (string tagCurrent in tags)
            {
                // Remove quebras de linha e espaços extras
                var tagActual = tagCurrent.Replace("\n", "").Replace("\r", "").Trim();

                // Separar palavras e eliminar tags com mais de três palavras
                var words = tagActual.Split(' ')
                                     .Where(word => !string.IsNullOrEmpty(word) && word.Length > 2 && !regex.IsMatch(word) && !pattern.IsMatch(word))
                                     .ToArray();
                if (words.Length <= 3)
                {
                    processedTags.AddRange(words);
                }
            }

            // Eliminar tags vazias e duplicadas
            return processedTags.Distinct().Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
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

    }
}