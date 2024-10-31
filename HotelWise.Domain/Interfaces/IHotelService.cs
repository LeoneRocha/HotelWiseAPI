using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IHotelService
    {
        Task AddHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(ulong id);
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(ulong id);
        Task UpdateHotelAsync(Hotel hotel);
    }
}