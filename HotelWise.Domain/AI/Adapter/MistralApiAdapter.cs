using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Mistral.SDK;
using Mistral.SDK.DTOs;

namespace HotelWise.Domain.AI.Adapter
{
    public class MistralApiAdapter : IAIInferenceAdapter
    {
        private readonly IApplicationConfig _applicationConfig;
        private readonly MistralClient _client;

        public MistralApiAdapter(IApplicationConfig applicationConfig)
        {
            _applicationConfig = applicationConfig;
            _client = new MistralClient(_applicationConfig.MistralApiConfig.ApiKey);
        }

        public async Task<string> GenerateChatCompletionAsync(string prompt)
        {
            var request = new ChatCompletionRequest(
                //define model - required
                ModelDefinitions.MistralMedium,
                //define messages - required
                new List<ChatMessage>()
                {
                    new ChatMessage(ChatMessage.RoleEnum.User, prompt)
                },
                //optional - defaults to false
                safePrompt: true,
                //optional - defaults to 0.7
                temperature: 0,
                //optional - defaults to null
                maxTokens: 500,
                //optional - defaults to 1
                topP: 1,
                //optional - defaults to null
                randomSeed: 32);
            var response = await _client.Completions.GetCompletionAsync(request);

            return response.VarObject.ToString();

        }

        public async Task<decimal[]> GenerateEmbeddingAsync(string text)
        {
            var request = new EmbeddingRequest(ModelDefinitions.MistralEmbed, new List<string>() { text }, EmbeddingRequest.EncodingFormatEnum.Float);

            var response = await _client.Embeddings.GetEmbeddingsAsync(request);

            //---PRECISO CONTINUAR
            //TODO: 
            //-- CRIAR UM ADAPTER E SERVICE SEMELHANTE AO CROC o emband nao vai ficar na classe do quadrand ele ja vei receber tudo ja embeend
            //-- REFATORA PARA O _vectorStoreService chamar o NOVO SERVICE DO MISTRAL QUE VAI SUBSTITUIR O GROQ pára gerar o embbaeding e chamar o COmpletion 

            var resultEmbedding = new List<decimal>();
            response.Data.ForEach(x => resultEmbedding.AddRange(x.Embedding.ToList()));

            return resultEmbedding.ToArray();
        }
    }
}