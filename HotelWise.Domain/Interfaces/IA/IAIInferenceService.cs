using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceService
    { 
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, EIAInferenceAdapterType eIAInferenceAdapterType);

        Task<float[]> GenerateEmbeddingAsync(string text, EIAInferenceAdapterType eIAInferenceAdapterType);
    }
}