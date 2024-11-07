namespace HotelWise.Domain.Interfaces
{
    public interface IAIInferenceService
    {
        Task<string> GenerateChatCompletionAsync(string prompt);
    }
}