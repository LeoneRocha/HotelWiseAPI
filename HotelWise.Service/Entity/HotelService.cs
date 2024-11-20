using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Helpers;
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
        private readonly Serilog.ILogger _logger;

        protected long UserId { get; private set; }


        public HotelService(
            Serilog.ILogger logger
            , IMapper mapper
            , IApplicationIAConfig applicationConfig
            , IHotelRepository hotelRepository
            , IGenerateHotelService generateHotelService
            , IVectorStoreService<HotelVector> hotelVectorStoreService)
        {
            _logger = logger;
            _mapper = mapper;
            _applicationConfig = applicationConfig;
            _hotelRepository = hotelRepository;
            _generateHotelService = generateHotelService;
            _hoteVectorStoreService = hotelVectorStoreService;
        }
          public void SetUserId(long id)
        {
            UserId = id;
        }
        public async Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync()
        {
            ServiceResponse<HotelDto[]> response = new ServiceResponse<HotelDto[]>();
            try
            {
                var hotels = await _hotelRepository.GetAll();
                var hotelDtos = _mapper.Map<HotelDto[]>(hotels);

                response.Data = hotelDtos.OrderBy(h=> h.HotelName).ToArray();
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetAllHotelsAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> InsertHotelInVectorStore(long id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(id);

                var hotelDto = _mapper.Map<HotelDto>(hotel);

                await addOrUpdateDataVector(hotelDto);
                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "InsertHotelInVectorStore: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<HotelDto?>> GetHotelByIdAsync(long id)
        {
            ServiceResponse<HotelDto?> response = new ServiceResponse<HotelDto?>();
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(id);

                var hotelDto = _mapper.Map<HotelDto?>(hotel);

                var hoteVector = await _hoteVectorStoreService.GetById(id);

                if (hoteVector != null && hotelDto != null)
                {
                    hotelDto.IsHotelInVectorStore = true;
                }
                response.Success = true;
                response.Data = hotelDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetHotelByIdAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        public async Task<ServiceResponse<HotelDto>> GenerateHotelByIA()
        {
            ServiceResponse<HotelDto> response = new ServiceResponse<HotelDto>();
            try
            {
                var hotel = await _generateHotelService.GetHotelAsync();

                var hotelDto = _mapper.Map<HotelDto>(hotel);

                response.Success = true;
                response.Data = hotelDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GenerateHotelByIA: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> AddHotelAsync(HotelDto hotelDto)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                #region Set default fields for bussines
                hotel.CreatedUserId = UserId;
                hotel.CreatedDate = DataHelper.GetDateTimeNow();
                hotel.ModifyDate = DataHelper.GetDateTimeNow(); 
                #endregion Set default fields for bussines
                 
                await _hotelRepository.AddAsync(hotel);

                hotelDto = _mapper.Map<HotelDto>(hotel);

                //Add Vector
                await addOrUpdateDataVector(hotelDto);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AddHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotelDto)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                #region Set default fields for bussines
                hotel.ModifyUserId = UserId;                
                hotel.ModifyDate = DataHelper.GetDateTimeNow();
                #endregion Set default fields for bussines

                await _hotelRepository.UpdateAsync(hotel);

                await addOrUpdateDataVector(hotelDto);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        private async Task addOrUpdateDataVector(HotelDto hotelDto)
        {
            if (hotelDto != null)
            {
                await _hoteVectorStoreService.UpsertDataAsync(convertHotelToVector(hotelDto));
            }
        }

        public async Task<ServiceResponse<bool>> DeleteHotelAsync(long id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                await _hotelRepository.DeleteAsync(id);

                await _hoteVectorStoreService.DeleteAsync(id);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "DeleteHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;

        }

        public async Task<ServiceResponse<HotelDto[]>> FetchHotelsAsync()
        {
            ServiceResponse<HotelDto[]> response = new ServiceResponse<HotelDto[]>();
            try
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
                var result = allHotels.Distinct().OrderBy(hotel => hotel.HotelId).ToArray();

                response.Success = true;
                response.Data = result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "FetchHotelsAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        public async Task<ServiceResponse<HotelDto[]>> SemanticSearch(SearchCriteria searchCriteria)
        {
            ServiceResponse<HotelDto[]> response = new ServiceResponse<HotelDto[]>();
            try
            {
                if (string.IsNullOrEmpty(searchCriteria.SearchTextCriteria))
                {
                    response.Success = false;
                    response.Data = [];
                    return response;
                }
                //TODO ENVIAR PARA UM CACHE to que pesquisar toda vez no banco de dados 
                var allHotels = (await FetchHotelsAsync()).Data;

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

                    var hotelEntity = allHotels!.FirstOrDefault(x => x.HotelId == hotelId);
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
                var result = resultHotels.OrderByDescending(h => h.Score).ToArray();

                response.Success = true;
                response.Data = result;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SemanticSearch: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;

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