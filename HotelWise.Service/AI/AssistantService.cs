using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Model;

namespace HotelWise.Service.Entity
{
    public class AssistantService : IAssistantService
    {
        private readonly IAIInferenceService _aIInferenceService;
        private readonly EIAInferenceAdapterType _eIAInferenceAdapterType;

        public AssistantService(IAIInferenceService aIInferenceService)
        {
            _eIAInferenceAdapterType = EIAInferenceAdapterType.GroqApi;
            _aIInferenceService = aIInferenceService;
        }

        public async Task<float[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
        }

        public async Task<AskAssistantResponse[]> AskAssistant(SearchCriteria searchCriteria)
        {
            var result = await _aIInferenceService.GenerateChatCompletionAsync(searchCriteria.SearchTextCriteria, _eIAInferenceAdapterType);
            return [new AskAssistantResponse() { Response = result }];
        }
    }
}