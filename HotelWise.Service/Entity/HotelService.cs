using HotelWise.Data.Context.Configure.Mock;
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

        public async Task<Hotel[]> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAll();
        }

        public async Task<Hotel?> GetHotelByIdAsync(long id)
        {
            return await _hotelRepository.GetByIdAsync(id);
        }

        public async Task<Hotel[]> GenerateHotelsByIA(int numberGerate)
        {
            var hotelsExists = await _hotelRepository.GetAllAsync();

            foreach (var hotel in hotelsExists)
            {
                hotel.Tags = HotelsMockData.ProcessTags(hotel.Tags);
            }
            await _hotelRepository.UpdateRangeAsync(hotelsExists);
             
            if (hotelsExists.Length < numberGerate)
            {
                var hotels = await HotelsMockData.GetHotelsAsync(numberGerate - hotelsExists.Length);

                await _hotelRepository.AddRangeAsync(hotels);
                List<Hotel> result = new List<Hotel>();
                result.AddRange(hotels);
                result.AddRange(hotelsExists);
                return result.ToArray();
            }

            return hotelsExists;
        }


        public async Task AddHotelAsync(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
        }

        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task DeleteHotelAsync(long id)
        {
            await _hotelRepository.DeleteAsync(id);
        }
    }

}
