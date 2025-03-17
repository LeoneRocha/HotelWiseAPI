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
    public class HotelSearchService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelSearchService
    {
        private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService;
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IHotelRepository _hotelRepository;
        public HotelSearchService(
            Serilog.ILogger logger,
            IMapper mapper,
            IApplicationIAConfig applicationConfig,
            IHotelRepository hotelRepository,
            IVectorStoreService<HotelVector> hotelVectorStoreService)
            : base(hotelRepository, mapper, logger)
        {
            _applicationConfig = applicationConfig;
            _hotelVectorStoreService = hotelVectorStoreService;
            _hotelRepository = hotelRepository;
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
                //NEXSTEP: ENVIAR PARA UM CACHE to que pesquisar toda vez no banco de dados 
                var allHotelsFromDb = (await fetchHotelsAsync()).Data;

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

        private async Task<ServiceResponse<HotelDto[]>> fetchHotelsAsync()
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
            // Mapear para novo objeto e retonar novo objeto NEXSTEP:
            var resultHotels = new List<HotelDto>();
            if (allHotelsFromDb != null && allHotelsFromDb.Length > 0 && hotelsVector != null && hotelsVector.Length > 0)
            {
                //Enriquecer com Interferencia IA  NEXSTEP:  TROCAR UMA PARTE POR MAPPER para facilitar
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
    }
}