namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapter
    {
        Task<decimal[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateChatCompletionAsync(string prompt);
    }
}