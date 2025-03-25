using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Interfaces.Entity.IA
{
    public interface IChatSessionHistoryRepository : IGenericRepository<ChatSessionHistory>
    {
        Task DeleteByIdTokenAsync(string token);
        Task<ChatSessionHistory?> GetByIdTokenAsync(string token);
    }
}
