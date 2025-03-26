using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;
using HotelWise.Service.Bussines;
using HotelWise.Service.Generic;
using HotelWise.Service.Prompts;
using System.Collections.Concurrent;

namespace HotelWise.Service.Entity
{
    public class HotelSearchService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelSearchService
    {
        private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService; 
        private readonly IHotelRepository _hotelRepository;
        private readonly IAIInferenceService _aIInferenceService;
        private readonly InferenceAiAdapterType _eIAInferenceAdapterType;

        public HotelSearchService(
            Serilog.ILogger logger,
            IMapper mapper,
            IApplicationIAConfig applicationConfig,
            IHotelRepository hotelRepository,
            IVectorStoreService<HotelVector> hotelVectorStoreService,
            IValidator<Hotel> entityValidator,
            IAIInferenceService aIInferenceService) 
            : base(hotelRepository, mapper, logger, entityValidator)
        { 
            _hotelVectorStoreService = hotelVectorStoreService;
            _hotelRepository = hotelRepository;
            _eIAInferenceAdapterType = applicationConfig.RagConfig.GetAInferenceAdapterType();
            _aIInferenceService = aIInferenceService;
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

                //Search Vector  
                await searchFromVector(searchCriteria, response, allHotelsFromDb);

                //SearchAndAnalyzePluginAsync GET FROM IA INTERFERENCE                  
                await searchByInterference(searchCriteria, response);

                // Processa a resposta da IA para obter os IDs dos hotéis inferidos
                var hotelsResultInterference = HotelResponseProcessor.ProcessResponse(response.Data!.PromptResultContent);

                // Filtra os resultados de HotelsVectorResult com base nos IDs retornados pela inferência
                response.Data = FilterHotelsByIAResult(response.Data!, hotelsResultInterference);

                if (response.Errors.Count == 0)
                {
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SemanticSearch: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
                response.Data.HotelsVectorResult = [];
                response.Data.HotelsIAResult = [];
            }
            return response;
        }

        public static HotelSemanticResult FilterHotelsByIAResult(HotelSemanticResult response, List<HotelInfo> hotelsResultInterference)
        {
            // Verifica se os dados de entrada estão válidos
            if (response == null || response.HotelsVectorResult == null || hotelsResultInterference == null)
                throw new ArgumentNullException("Os parâmetros de entrada não podem ser nulos.");

            // Lista de IDs retornados pela inferência
            var interferenceIds = new HashSet<long>(hotelsResultInterference.Select(h => h.Id));

            // Filtra os hotéis do vetor com base nos IDs
            var hotelsMatch = response.HotelsVectorResult.Where(hotel => interferenceIds.Contains(hotel.HotelId)).ToArray();
            response.HotelsVectorResult = hotelsMatch;  
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

        private async Task searchByInterference(SearchCriteria searchCriteria, ServiceResponse<HotelSemanticResult> response)
        {
            PromptMessageVO[] historyPrompts = createPrompts(searchCriteria, response.Data!.HotelsVectorResult);
            var result = await _aIInferenceService.GenerateChatCompletionByAgentSimpleRagAsync(historyPrompts, _eIAInferenceAdapterType);

            response.Data!.PromptResultContent = result;

            HotelDto[] listHotelsIAInterference = changeHotelsVectorToHotelDtos(response.Data!.HotelsVectorResult, []);
            response.Data!.HotelsIAResult = listHotelsIAInterference;

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

        private static PromptMessageVO[] createPrompts(SearchCriteria request, HotelDto[]? allHotelsFromDb)
        {
            PromptMessageVO sysMsgHotelAgent = StayMatePromptGenerator.CreateHotelAgentPrompt();
            PromptMessageVO sysMsgHotelSearch = StayMatePromptGenerator.CreateHotelSystemPrompt();

            PromptMessageVO ragMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.Context,
                DataContextRag = convertDataContext(allHotelsFromDb)
            };

            PromptMessageVO userMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.User,
                Content = request.SearchTextCriteria
            };

            PromptMessageVO[] messages = [sysMsgHotelAgent, sysMsgHotelSearch, userMsg, ragMsg];
            return messages;
        }

        private static DataVectorVO[] convertDataContext(HotelDto[]? allHotelsFromDb)
        {
            List<DataVectorVO> dataVectorVOs = new List<DataVectorVO>();
            foreach (var hotelDto in allHotelsFromDb)
            {
                dataVectorVOs.Add(new DataVectorVO()
                {
                    DataVector = string.Format("Hotel Description: {0} Hotel Id: {1}", hotelDto.Description, hotelDto.HotelId),
                    KeyVector = hotelDto.HotelId.ToString()
                });
            }
            return dataVectorVOs.ToArray();
        }
    }
}