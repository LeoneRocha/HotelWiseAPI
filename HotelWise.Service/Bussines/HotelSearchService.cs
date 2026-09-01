using System.Collections.Concurrent;
using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Bussines;
using HotelWise.Service.Prompts;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de busca semântica inteligente de hotéis, combinando busca vetorial com análise e síntese via modelo de linguagem (RAG).
/// </summary>
public class HotelSearchService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelSearchService
{
    private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService;
    private readonly IHotelRepository _hotelRepository;
    private readonly IAIInferenceService _aIInferenceService;
    private readonly InferenceAiAdapterType _eIAInferenceAdapterType;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelSearchService"/> com as dependências de Vector Store, repositório e inferência de IA.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="mapper">Mapeador de objetos AutoMapper.</param>
    /// <param name="applicationConfig">Configuração de IA.</param>
    /// <param name="hotelRepository">Repositório de dados de hotéis.</param>
    /// <param name="hotelVectorStoreService">Serviço de busca vetorial.</param>
    /// <param name="entityValidator">Validador de hotel.</param>
    /// <param name="aIInferenceService">Serviço de inferência de IA.</param>
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

    /// <summary>
    /// Executa o fluxo completo de busca semântica: busca vetorial, enriquecimento contextual e inferência via LLM com StayMate.
    /// </summary>
    /// <param name="searchCriteria">Critérios e parâmetros de busca informados pelo usuário.</param>
    /// <returns>Resposta contendo o resultado semântico consolidado (<see cref="HotelSemanticResult"/>).</returns>
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

            // Se o chamador não especificou um limite ou passou <= 0, usa o total de hotéis do banco ou padrão amplo (1000) para não cortar resultados
            if (searchCriteria.MaxRetrieve <= 0)
            {
                searchCriteria.MaxRetrieve = (allHotelsFromDb != null && allHotelsFromDb.Length > 0)
                    ? allHotelsFromDb.Length
                    : 1000;
            }

            //Search Vector  
            await searchFromVector(searchCriteria, response, allHotelsFromDb);

            //SearchAndAnalyzePluginAsync GET FROM IA INTERFERENCE                  
            await searchByInterference(searchCriteria, response);

            // Processa a resposta da IA para obter os IDs dos hotéis inferidos
            var hotelsResultInterference = HotelResponseProcessor.ProcessResponse(response.Data.PromptResultContent);

            // Filtra os resultados de HotelsVectorResult com base nos IDs retornados pela inferência
            response.Data = FilterHotelsByIAResult(response.Data, hotelsResultInterference);

            if (response.Errors.Count == 0)
            {
                response.Success = true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "SemanticSearch: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            response.Data.HotelsVectorResult = [];
            response.Data.HotelsIAResult = [];
        }
        return response;
    }

    /// <summary>
    /// Filtra a lista de hotéis recuperados do Vector Store mantendo apenas os identificados pela inferência da IA.
    /// </summary>
    /// <param name="response">Resultado semântico atual.</param>
    /// <param name="hotelsResultInterference">Array de metadados extraídos da resposta do LLM.</param>
    /// <returns>Resultado semântico filtrado.</returns>
    public static HotelSemanticResult FilterHotelsByIAResult(HotelSemanticResult response, HotelInfo[] hotelsResultInterference)
    {
        // Verifica se os dados de entrada estão válidos
        if (response == null || response.HotelsVectorResult == null || hotelsResultInterference == null)
            throw new InvalidOperationException("Os parâmetros de entrada não podem ser nulos.");

        // Lista de IDs retornados pela inferência
        var interferenceIds = new HashSet<long>(hotelsResultInterference.Select(h => h.Id));

        // Filtra os hotéis do vetor com base nos IDs
        var hotelsMatch = response.HotelsVectorResult.Where(hotel => interferenceIds.Contains(hotel.HotelId)).ToArray();
        response.HotelsVectorResult = hotelsMatch;
        return response;
    }

    /// <summary>
    /// Busca hotéis do banco de dados de forma paginada e concorrente.
    /// </summary>
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
            _logger.Error(ex, "FetchHotelsAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Consulta os hotéis na base vetorial e correlaciona os dados com a base relacional.
    /// </summary>
    private async Task searchFromVector(SearchCriteria searchCriteria, ServiceResponse<HotelSemanticResult> response, HotelDto[]? allHotelsFromDb)
    {
        var responseVector = await _hotelVectorStoreService.VectorizedSearchAsync(searchCriteria);
        var hotelsVector = responseVector.Data;
        HotelDto[] listHotelsVector = changeHotelsVectorToHotelDtos(allHotelsFromDb, hotelsVector);
        response.Data.HotelsVectorResult = listHotelsVector;
        response.Errors.AddRange(responseVector.Errors);
        response.Message = responseVector.Message;
    }

    /// <summary>
    /// Envia os hotéis do contexto para o agente de IA para geração da recomendação de viagem.
    /// </summary>
    private async Task searchByInterference(SearchCriteria searchCriteria, ServiceResponse<HotelSemanticResult> response)
    {
        PromptMessageVO[] historyPrompts = createPrompts(searchCriteria, response.Data.HotelsVectorResult);

        // valida prompts  
        var promptsValidator = new HistoryPromptsValidator();
        var promptsValidationResult = promptsValidator.Validate(historyPrompts);
        if (!promptsValidationResult.IsValid)
        {
            throw new ValidationException(promptsValidationResult.Errors);
        }

        var result = await _aIInferenceService.GenerateChatCompletionByAgentSimpleRagAsync(historyPrompts, _eIAInferenceAdapterType);

        response.Data.PromptResultContent = result;

        HotelDto[] listHotelsIAInterference = changeHotelsVectorToHotelDtos(response.Data.HotelsVectorResult, []);
        response.Data.HotelsIAResult = listHotelsIAInterference;
    }

    /// <summary>
    /// Converte e mescla registros vetoriais com informações detalhadas do banco relacional.
    /// </summary>
    private static HotelDto[] changeHotelsVectorToHotelDtos(HotelDto[]? allHotelsFromDb, HotelVector[]? hotelsVector)
    {
        var resultHotels = new List<HotelDto>();
        if (allHotelsFromDb != null && allHotelsFromDb.Length > 0 && hotelsVector != null && hotelsVector.Length > 0)
        {
            foreach (var hotelVector in hotelsVector)
            {
                var hotelId = (long)hotelVector.DataKey;

                var hotelEntity = allHotelsFromDb.FirstOrDefault(x => x.HotelId == hotelId);
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

    /// <summary>
    /// Constrói os prompts e o contexto RAG para alimentar a inferência do LLM.
    /// </summary>
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
            Content = request.SearchTextCriteria,
        };
        PromptMessageVO[] messages = [sysMsgHotelAgent, sysMsgHotelSearch, userMsg, ragMsg];
        return messages;
    }

    /// <summary>
    /// Converte uma lista de hotéis DTO em objetos de vetor de contexto para o pipeline RAG.
    /// </summary>
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