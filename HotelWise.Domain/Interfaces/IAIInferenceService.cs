namespace HotelWise.Domain.Interfaces
{
    public interface IAIInferenceService
    {
        Task<string> GenerateDescriptionAndTagsAsync(string prompt);
    }
}