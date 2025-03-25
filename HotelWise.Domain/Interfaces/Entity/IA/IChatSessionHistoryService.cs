using HotelWise.Domain.Dto.IA;
using HotelWise.Service.Generic;

namespace HotelWise.Domain.Interfaces.Entity.IA
{
    public interface IChatSessionHistoryService : IGenericService<ChatSessionHistoryDto>
    {
        void SetUserId(long id);
        Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string id);
        Task DeleteByIdTokenAsync(string id);

    }
}