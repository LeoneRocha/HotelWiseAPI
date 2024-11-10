using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;
using System.Collections.Concurrent;

namespace HotelWise.Service.Entity
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IGenerateHotelService _generateHotelService;
        private readonly IVectorStoreService _vectorStoreService;

        public HotelService(IHotelRepository hotelRepository
            , IGenerateHotelService generateHotelService
            , IVectorStoreService vectorStoreService)
        {
            _hotelRepository = hotelRepository;
            _generateHotelService = generateHotelService;
            _vectorStoreService = vectorStoreService;
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
                hotel.Tags = GenerateHotelService.ProcessTags(hotel.Tags);
            }
            await _hotelRepository.UpdateRangeAsync(hotelsExists);

            if (hotelsExists.Length < numberGerate)
            {
                var totalAdd = numberGerate - hotelsExists.Length;
                var hotels = await _generateHotelService.GetHotelsAsync(totalAdd);

                await _hotelRepository.AddRangeAsync(hotels);
                List<Hotel> result = new List<Hotel>(hotels);
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

        public async Task<Hotel[]> FetchHotelsAsync()
        {
            int batchSize = 10;
            var allHotels = new ConcurrentBag<Hotel>();

            int totalHotels = await _hotelRepository.GetTotalHotelsCountAsync();
            int fromCount = 0;
            int toCount = (totalHotels + batchSize - 1) / batchSize;

            await Parallel.ForEachAsync(Enumerable.Range(fromCount, toCount - fromCount), async (index, cancellationToken) =>
            {
                var hotels = await _hotelRepository.FetchHotelsAsync(index * batchSize, batchSize);
                foreach (var hotel in hotels)
                {
                    allHotels.Add(hotel);
                }
            });

            return allHotels.Distinct().OrderBy(hotel => hotel.HotelId).ToArray();
        }

        public async Task<Hotel[]> SemanticSearch(SearchCriteria searchCriteria)
        {
            var allHotels = await FetchHotelsAsync();
            allHotels = allHotels.Take(1).ToArray();
             
            //Add vactor
            await _vectorStoreService.UpsertHotelAsync(allHotels);

            var testGetVector = await _vectorStoreService.GetById(1);

            //Search e IA
            //var result = await _vectorStoreService.SearchHotelsAsync(searchCriteria.SearchTextCriteria);

            return allHotels;
        }
    }
}