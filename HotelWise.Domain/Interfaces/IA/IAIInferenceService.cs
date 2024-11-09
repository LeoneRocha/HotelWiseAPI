namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceService
    {
        Task<string> GenerateChatCompletionAsync(string prompt);
    }
}