namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapter
    {
        Task<string> GenerateChatCompletionAsync(string prompt);
    }
}