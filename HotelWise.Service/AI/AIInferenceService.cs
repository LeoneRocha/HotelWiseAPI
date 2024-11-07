using HotelWise.Domain.AI.Models;
using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Service.AI
{
    public class AIInferenceService : IAIInferenceService
    {
        private readonly string _apiKey;
        private readonly IAIInferenceAdapterFactory _adapterFactory;

        public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
        {
            _apiKey = configuration["GroqApi:ApiKey"]!;
            _adapterFactory = adapterFactory;
        }
        public async Task<string> GenerateChatCompletionAsync(string prompt)
        {
            var model = new MixtralModelStrategy();
            var adapter = _adapterFactory.CreateAdapter(_apiKey, model);
            return await adapter.GenerateChatCompletionAsync(prompt);
        }
    }
}