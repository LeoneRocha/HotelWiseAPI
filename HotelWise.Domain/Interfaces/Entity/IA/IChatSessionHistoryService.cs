using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.IA
{
    public interface IChatSessionHistoryService : IGenericService<ChatSessionHistoryDto>
    {
        void SetUserId(long id);
        Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string token);
        Task DeleteByIdTokenAsync(string token);
    }
}