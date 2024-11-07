namespace HotelWise.Domain.Interfaces
{ 
    public interface IAIInferenceAdapter
    {
        Task<string> GenerateChatCompletionAsync(string prompt);
    } 
} 