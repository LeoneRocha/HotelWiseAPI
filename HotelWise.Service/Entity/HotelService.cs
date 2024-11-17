using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces;
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
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IMapper _mapper;
        public HotelService(
            IMapper mapper,
            IApplicationIAConfig applicationConfig
            , IHotelRepository hotelRepository
            , IGenerateHotelService generateHotelService
            , IVectorStoreService<HotelVector> hotelVectorStoreService)
        {
            _mapper = mapper;
            _applicationConfig = applicationConfig;
            _hotelRepository = hotelRepository;
            _generateHotelService = generateHotelService;
            _hoteVectorStoreService = hotelVectorStoreService;
        }

        public async Task<HotelDto[]> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAll();

            var hotelDtos = _mapper.Map<HotelDto[]>(hotels);

            return hotelDtos;
        }

        public async Task<bool> InsertHotelInVectorStore(long id)
        { 
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(id);

                var hotelDto = _mapper.Map<HotelDto>(hotel);
                  
                await addOrUpdateDataVector(hotelDto);
                return true;
            }
            catch (Exception)
            {
                throw;
            } 
        } 
        public async Task<HotelDto?> GetHotelByIdAsync(long id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            var hoteVector = await _hoteVectorStoreService.GetById(id);

            if (hoteVector != null) {
                hotelDto.IsHotelInVectorStore = true;
            } 
            return hotelDto;
        }

        public async Task<HotelDto> GenerateHotelByIA()
        {
            var hotel = await _generateHotelService.GetHotelAsync();

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            return hotelDto;
        }
        public async Task AddHotelAsync(HotelDto hotelDto)
        {
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                await _hotelRepository.AddAsync(hotel);

                //Add Vector
                await addOrUpdateDataVector(hotelDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateHotelAsync(HotelDto hotelDto)
        {
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                await _hotelRepository.UpdateAsync(hotel);

                await addOrUpdateDataVector(hotelDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task addOrUpdateDataVector(HotelDto hotelDto)
        {
            if (hotelDto != null)
            {
                await _hoteVectorStoreService.UpsertDataAsync(convertHotelToVector(hotelDto));
            }
        }

        public async Task DeleteHotelAsync(long id)
        {
            try
            {
                await _hotelRepository.DeleteAsync(id);

                await _hoteVectorStoreService.DeleteAsync(id);
            }
            catch (Exception)
            { 
                throw;
            }

        }

        public async Task<HotelDto[]> FetchHotelsAsync()
        {
            int batchSize = 10;
            var allHotels = new ConcurrentBag<HotelDto>();

            int totalHotels = await _hotelRepository.GetTotalHotelsCountAsync();
            int fromCount = 0;
            int toCount = (totalHotels + batchSize - 1) / batchSize;

            await Parallel.ForEachAsync(Enumerable.Range(fromCount, toCount - fromCount), async (index, cancellationToken) =>
            {
                var hotels = await _hotelRepository.FetchHotelsAsync(index * batchSize, batchSize);
                var hotelDtos = _mapper.Map<HotelDto[]>(hotels);

                foreach (var hotel in hotelDtos)
                {
                    allHotels.Add(hotel);
                }
            });
            return allHotels.Distinct().OrderBy(hotel => hotel.HotelId).ToArray();
        }

        public async Task<HotelDto[]> SemanticSearch(SearchCriteria searchCriteria)
        {
            //TODO ENVIAR PARA UM CACHE to que pesquisar toda vez no banco de dados 
            var allHotels = await FetchHotelsAsync();
            allHotels = allHotels.Take(searchCriteria.MaxHotelRetrieve).ToArray();

            HotelVector[] hotelsVectorStore = convertHotelsToVector(allHotels);
            //Add vactor
            await _hoteVectorStoreService.UpsertDatasAsync(hotelsVectorStore);//TODO REMOVER DEPOIS QUE TIVER SALVO NO vector -- TODO CRIAR UMA ABORDAGEM DE SALVAR NO MONGO OU SIMILAR

            // Aguardar o tempo configurado antes de buscar o vetor
            await Task.Delay(_applicationConfig.RagConfig.SearchSettings.DelayBeforeSearchMilliseconds);

            //Search Vector
            var hotelsVector = await _hoteVectorStoreService.SearchDatasAsync(searchCriteria.SearchTextCriteria);

            //ENRRIQUECER COM IA TODO: 

            // Mapear para novo objeto e retonar novo objeto TODO:
            var resultHotels = new List<HotelDto>();
            //Enriquecer com Interferencia IA  TODO:  TROCAR UMA PARTE POR MAPPER para facilitar
            foreach (var hotelVector in hotelsVector)
            {
                var hotelId = (long)hotelVector.DataKey;

                var hotelEntity = allHotels.FirstOrDefault(x => x.HotelId == hotelId);
                if (hotelEntity != null)
                {
                    var hotelResponse = new HotelDto()
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
            }
            return resultHotels.ToArray();
        }

        private HotelVector[] convertHotelsToVector(HotelDto[] allHotels)
        {
            List<HotelVector> hotelsVectorStore = new List<HotelVector>();
            foreach (var hotel in allHotels)
            {
                hotelsVectorStore.Add(convertHotelToVector(hotel));
            }
            return hotelsVectorStore.ToArray();
        }

        private HotelVector convertHotelToVector(HotelDto hotel)
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