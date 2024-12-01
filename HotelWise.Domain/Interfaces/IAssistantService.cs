using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IAssistantService
    {
        Task<float[]?> GenerateEmbeddingAsync(string text);

        Task<AskAssistantResponse[]> AskAssistant(SearchCriteria searchCriteria);
        void SetUserId(long v);
    }
}
