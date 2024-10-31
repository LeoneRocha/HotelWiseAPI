using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Model;

namespace HotelWise.Service.Entity
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(ulong id)
        {
            return await _hotelRepository.GetByIdAsync(id);
        }

        public async Task AddHotelAsync(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
        }

        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task DeleteHotelAsync(ulong id)
        {
            await _hotelRepository.DeleteAsync(id);
        }
    }

}
