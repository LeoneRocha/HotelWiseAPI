using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceService
    { 
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);

        Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType);
    }
}