using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(ulong id);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(ulong id);
    }

}
