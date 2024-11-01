using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel[]> GetAllAsync();
        Task<Hotel?> GetByIdAsync(long id);
        Task AddAsync(Hotel hotel);
        Task AddRangeAsync(Hotel[] hotels);

        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(long id);
    }

}
