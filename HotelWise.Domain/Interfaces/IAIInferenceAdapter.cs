namespace HotelWise.Domain.Interfaces
{ 
    public interface IAIInferenceAdapter
    {
        Task<string> GenerateDescriptionAndTagsAsync(string prompt);
    } 
} 