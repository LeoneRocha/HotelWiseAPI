using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;
using HotelWise.Service.Generic;
using System.Collections.Concurrent;

namespace HotelWise.Service.Entity
{
    public class HotelService : GenericServiceBase<Hotel, HotelDto>, IHotelService
    { 
        private readonly IGenerateHotelService _generateHotelService;
        private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService;
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IHotelRepository _hotelRepository;

      
        public HotelService(
            Serilog.ILogger logger,
            IMapper mapper,
            IApplicationIAConfig applicationConfig,
            IHotelRepository hotelRepository,
            IGenerateHotelService generateHotelService,
            IVectorStoreService<HotelVector> hotelVectorStoreService)
            : base(hotelRepository, mapper, logger)
        { 
            _applicationConfig = applicationConfig;
            _generateHotelService = generateHotelService;
            _hotelVectorStoreService = hotelVectorStoreService;
            _hotelRepository = hotelRepository;
        } 
        public async Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync()
        {
            ServiceResponse<HotelDto[]> response = new ServiceResponse<HotelDto[]>();
            try
            {
                var hotels = await _hotelRepository.GetAllAsync();
                var hotelDtos = _mapper.Map<HotelDto[]>(hotels);

                response.Data = hotelDtos.OrderBy(h => h.HotelName).ToArray();
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

                var hoteVector = await _hotelVectorStoreService.GetById(id);

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

                handleTagsBeforeSave(hotel);

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

                //Padronizar tags
                handleTagsBeforeSave(hotel);

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

        public async Task<string[]> GetAllTags()
        {
            List<string> tagsResult = new List<string>();
            try
            {
                int batchSize = 10;
                var allTagsConcurrentBag = new ConcurrentBag<List<string>>();

                int totalHotels = await _hotelRepository.GetTotalHotelsCountAsync();
                int fromCount = 0;
                int toCount = (totalHotels + batchSize - 1) / batchSize;

                await Parallel.ForEachAsync(Enumerable.Range(fromCount, toCount - fromCount), async (index, cancellationToken) =>
                {
                    var tags = await _hotelRepository.GetAllTagsAsync(index * batchSize, batchSize);

                    foreach (var tag in tags)
                    {
                        allTagsConcurrentBag.Add(tag.Select(x => x.ToLower()).ToList());
                    }
                });

                foreach (var tagsbag in allTagsConcurrentBag)
                {
                    tagsResult.AddRange(tagsbag);
                }
                var result = tagsResult.Distinct().OrderBy(tag => tag).ToList();
                tagsResult = result.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "FetchHotelsAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
            return tagsResult.ToArray();
        }

        public async Task<ServiceResponse<bool>> DeleteHotelAsync(long id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                await _hotelRepository.DeleteAsync(id);

                await _hotelVectorStoreService.DeleteAsync(id);

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

        public async Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria)
        {
            ServiceResponse<HotelSemanticResult> response = new ServiceResponse<HotelSemanticResult>();
            response.Data = new HotelSemanticResult();

            try
            {
                if (string.IsNullOrEmpty(searchCriteria.SearchTextCriteria))
                {
                    response.Success = false;
                    return response;
                }
                //TODO ENVIAR PARA UM CACHE to que pesquisar toda vez no banco de dados 
                var allHotelsFromDb = (await FetchHotelsAsync()).Data;

                // Aguardar o tempo configurado antes de buscar o vetor
                await Task.Delay(_applicationConfig.RagConfig.SearchSettings.DelayBeforeSearchMilliseconds);

                //Search Vector  
                await searchFromVector(searchCriteria, response, allHotelsFromDb);

                //SearchAndAnalyzePluginAsync GET FROM IA INTERFERENCE                  
                await searchByPluguin(searchCriteria, response, allHotelsFromDb);

                if (response.Errors.Count == 0)
                {
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SemanticSearch: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }

        private static void handleTagsBeforeSave(Hotel hotel)
        {
            hotel.Tags = hotel.Tags.Select(t => t.ToLower().Trim()).ToArray();
        }

        private async Task addOrUpdateDataVector(HotelDto hotelDto)
        {
            if (hotelDto != null)
            {
                await _hotelVectorStoreService.UpsertDataAsync(convertHotelToVector(hotelDto));
            }
        }

        private async Task searchFromVector(SearchCriteria searchCriteria, ServiceResponse<HotelSemanticResult> response, HotelDto[]? allHotelsFromDb)
        {
            var responseVector = await _hotelVectorStoreService.VectorizedSearchAsync(searchCriteria);
            var hotelsVector = responseVector.Data;
            HotelDto[] listHotelsVector = changeHotelsVectorToHotelDtos(allHotelsFromDb, hotelsVector);
            response.Data!.HotelsVectorResult = listHotelsVector;
            response.Errors.AddRange(responseVector.Errors);
            response.Message = responseVector.Message;
        }

        private async Task searchByPluguin(SearchCriteria searchCriteria, ServiceResponse<HotelSemanticResult> response, HotelDto[]? allHotelsFromDb)
        {
            var responseIAInterference = await _hotelVectorStoreService.SearchAndAnalyzePluginAsync(searchCriteria.SearchTextCriteria);
            var hotelsIAInterference = responseIAInterference.Data;
            HotelDto[] listHotelsIAInterference = changeHotelsVectorToHotelDtos(allHotelsFromDb, hotelsIAInterference);
            response.Data!.HotelsIAResult = listHotelsIAInterference;
            response.Errors.AddRange(responseIAInterference.Errors);
            response.Message = responseIAInterference.Message;
        }

        private static HotelDto[] changeHotelsVectorToHotelDtos(HotelDto[]? allHotelsFromDb, HotelVector[]? hotelsVector)
        {
            // Mapear para novo objeto e retonar novo objeto TODO:
            var resultHotels = new List<HotelDto>();
            if (allHotelsFromDb != null && allHotelsFromDb.Length > 0 && hotelsVector != null && hotelsVector.Length > 0)
            {
                //Enriquecer com Interferencia IA  TODO:  TROCAR UMA PARTE POR MAPPER para facilitar
                foreach (var hotelVector in hotelsVector)
                {
                    var hotelId = (long)hotelVector.DataKey;

                    var hotelEntity = allHotelsFromDb!.FirstOrDefault(x => x.HotelId == hotelId);
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
                return result;
            }
            return [];
        }

        private static HotelVector convertHotelToVector(HotelDto hotel)
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