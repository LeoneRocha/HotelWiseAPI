namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapter
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateChatCompletionAsync(string prompt);
    }
}