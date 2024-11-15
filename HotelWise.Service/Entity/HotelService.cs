﻿using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
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
        private readonly IVectorStoreService<HotelVector> _hoteVectorStoreService;

        public HotelService(IHotelRepository hotelRepository
            , IGenerateHotelService generateHotelService
            , IVectorStoreService<HotelVector> hotelVectorStoreService)
        {
            _hotelRepository = hotelRepository;
            _generateHotelService = generateHotelService;
            _hoteVectorStoreService = hotelVectorStoreService;
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
            await addOrUpdateDataVector(hotel);
            await _hotelRepository.AddAsync(hotel);
        }

        private async Task addOrUpdateDataVector(Hotel hotel)
        {
            if (hotel != null)
            {
                await _hoteVectorStoreService.UpsertDataAsync(convertHotelToVector(hotel));
            }
        }

        public async Task UpdateHotelAsync(Hotel hotel)
        {
            await addOrUpdateDataVector(hotel);
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

        public async Task<HotelResponse[]> SemanticSearch(SearchCriteria searchCriteria)
        {
            //TODO ENVIAR PARA UM CACHE to que pesquisar toda vez no banco de dados 
            var allHotels = await FetchHotelsAsync();
            allHotels = allHotels.Take(searchCriteria.MaxHotelRetrive).ToArray();

            HotelVector[] hotelsVectorStore = convertHotelsToVector(allHotels);
            //Add vactor
            await _hoteVectorStoreService.UpsertDatasAsync(hotelsVectorStore);//TODO REMOVER DEPOIS QUE TIVER SALVO NO vector -- TODO CRIAR UMA ABORDAGEM DE SALVAR NO MONGO OU SIMILAR

            //Search Vector
            var hotelsVector = await _hoteVectorStoreService.SearchDatasAsync(searchCriteria.SearchTextCriteria);

            //ENRRIQUECER COM IA TODO: 

            // Mapear para novo objeto e retonar novo objeto TODO:
            var resultHotels = new List<HotelResponse>();
            //Enriquecer com Interferencia IA  TODO:  TROCAR UMA PARTE POR MAPPER para facilitar
            foreach (var hotelVector in hotelsVector)
            {
                var hotelId = (long)hotelVector.DataKey;

                var hotelEntity = allHotels.First(x => x.HotelId == hotelId);

                var hotelResponse = new HotelResponse()
                {
                    HotelId = hotelId,
                    Description = hotelVector.Description,
                    HotelName = hotelVector.HotelName,
                    Score = hotelVector.Score,
                    City = hotelEntity.City,
                    InitialRoomPrice = hotelEntity.InitialRoomPrice,
                    Location = hotelEntity.Location,
                    Stars = hotelEntity.Stars,
                    StateCode = hotelEntity.StateCode,
                    Tags = hotelEntity.Tags,
                    ZipCode = hotelEntity.ZipCode
                };

                resultHotels.Add(hotelResponse);
            }
            return resultHotels.ToArray();
        }

        private HotelVector[] convertHotelsToVector(Hotel[] allHotels)
        {
            List<HotelVector> hotelsVectorStore = new List<HotelVector>();
            foreach (var hotel in allHotels)
            {
                hotelsVectorStore.Add(convertHotelToVector(hotel));
            }
            return hotelsVectorStore.ToArray();
        }

        private HotelVector convertHotelToVector(Hotel hotel)
        {
            return new HotelVector()
            {
                DataKey = (ulong)hotel.HotelId,
                Description = hotel.Description,
                HotelName = hotel.HotelName,
                Tags = hotel.Tags.ToList()
            };
        }
    }
}