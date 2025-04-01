using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<int> GetTotalHotelsCountAsync();
        Task<Hotel[]> FetchHotelsAsync(int offset, int limit);
        Task<string[][]> GetAllTagsAsync(int offset, int limit);
    }
}
