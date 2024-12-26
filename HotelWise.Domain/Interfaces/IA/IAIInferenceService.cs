using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceService
    { 
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, IAInferenceAdapterType eIAInferenceAdapterType);

        Task<float[]> GenerateEmbeddingAsync(string text, IAInferenceAdapterType eIAInferenceAdapterType);
    }
}