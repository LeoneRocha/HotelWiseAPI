using HotelWise.Domain.Dto.IA;

namespace HotelWise.Domain.Interfaces.Entity.IA
{
    public interface IChatSessionHistoryService : IGenericService<ChatSessionHistoryDto>
    { 
        Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string token);
        Task DeleteByIdTokenAsync(string token);
    }
}
