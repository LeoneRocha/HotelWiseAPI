using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceService
    { 
        Task<string> GenerateChatCompletionAsync(string prompt, EIAInferenceAdapterType eIAInferenceAdapterType);

        Task<float[]> GenerateEmbeddingAsync(string text, EIAInferenceAdapterType eIAInferenceAdapterType);
    }
}