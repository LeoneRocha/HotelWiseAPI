using AutoMapper;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using Microsoft.Extensions.Configuration;
using SmartCoreHub.Core.SDK.Domain.AI.Configuration;

namespace HotelWise.Service.AI;

/// <summary>
/// Serviço de armazenamento e busca vetorial de hotéis (<see cref="HotelVector"/>), integrando geração de embeddings e consultas por similaridade.
/// </summary>
public class HotelVectorStoreService : GenericVectorStoreServiceBase, IVectorStoreService<HotelVector>
{
    private readonly IVectorStoreAdapter<HotelVector> _adapter;
    private readonly IAIInferenceService _aIInferenceService;
    private readonly string nameCollection;
    private readonly InferenceAiAdapterType _eIAInferenceAdapterType;
    private readonly IConfiguration? _configuration;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelVectorStoreService"/> com os adaptadores de inferência e de armazenamento vetorial.
    /// </summary>
    public HotelVectorStoreService(
        Serilog.ILogger logger,
        IMapper mapper,
        IApplicationIAConfig applicationIAConfig,
        IVectorStoreAdapterFactory adapterFactory,
        IAIInferenceService aIInferenceService,
        IConfiguration? configuration = null) : base(mapper, logger)
    {
        _configuration = configuration;
        _eIAInferenceAdapterType = applicationIAConfig.RagConfig.GetAInferenceAdapterType();
        _adapter = adapterFactory.CreateAdapter<HotelVector>();
        _aIInferenceService = aIInferenceService;

        // Prefixo + dimensão (opcional) via ApplicationIAConfig:Rag
        nameCollection = applicationIAConfig.RagConfig.BuildCollectionName("skhotels");
    }

    /// <summary>
    /// Gera embeddings vetoriais para o texto informado.
    /// </summary>
    public async Task<float[]?> GenerateEmbeddingAsync(string text)
    {
        return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
    }

    /// <summary>
    /// Obtém um registro vetorial de hotel a partir de sua chave identificadora.
    /// </summary>
    public async Task<HotelVector?> GetById(long dataKey)
    {
        try
        {
            var hotelVector = await _adapter.GetByKey(nameCollection, (ulong)dataKey);

            if (hotelVector != null)
            {
                return hotelVector;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "HotelVectorStoreService GetById: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
        }
        return null;
    }

    /// <summary>
    /// Insere ou atualiza um registro vetorial de hotel calculando automaticamente seu embedding.
    /// </summary>
    public async Task UpsertDataAsync(HotelVector entity)
    {
        var embedding = await _aIInferenceService.GenerateEmbeddingAsync(entity.Description, _eIAInferenceAdapterType);
        entity.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);
        await _adapter.UpsertDataAsync(nameCollection, entity);
    }

    /// <summary>
    /// Insere ou atualiza múltiplos registros vetoriais de hotéis em lote.
    /// </summary>
    public async Task UpsertDatasAsync(HotelVector[] listEntity)
    {
        var hotelVectors = new List<HotelVector>();

        foreach (HotelVector hotel in listEntity)
        {
            if (!await _adapter.Exists(nameCollection, hotel.DataKey))
            {
                var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, _eIAInferenceAdapterType);
                hotel.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);
                hotelVectors.Add(hotel);
            }
        }
        if (hotelVectors.Count > 0)
        {
            await _adapter.UpsertDatasAsync(nameCollection, hotelVectors.ToArray());
        }
    }

    /// <summary>
    /// Executa uma busca vetorial baseada na similaridade de cossenos para os critérios especificados.
    /// </summary>
    public async Task<SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>> VectorizedSearchAsync(SearchCriteria searchCriteria)
    {
        try
        {
            if (searchCriteria.MaxRetrieve <= 0)
            {
                var configuredMax = _configuration?.GetValue<int?>("ApplicationIAConfig:Rag:SearchSettings:MaxRetrieve") ?? 0;
                searchCriteria.MaxRetrieve = configuredMax > 0 ? configuredMax : 25;
            }

            var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchCriteria.SearchTextCriteria, _eIAInferenceAdapterType);
            var hotelsVector = await _adapter.VectorizedSearchAsync(nameCollection, embeddingSearchText, searchCriteria);
            return SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>.Ok(hotelsVector);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred in VectorizedSearchAsync at: {Message} at: {Time}", ex.Message, DateTime.UtcNow);

#pragma warning disable S6776
            return SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>.Error(
                [new SmartCoreHub.Core.SDK.Domain.DTOs.Common.ErrorResponse { Message = ex.Message }],
                ex.Message);
#pragma warning restore S6776
        }
    }

    /// <summary>
    /// Executa busca e análise combinada com plugins do Semantic Kernel para interpretação de intenção de busca.
    /// </summary>
    public async Task<SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>> SearchAndAnalyzePluginAsync(string searchText)
    {
        try
        {
            var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchText, _eIAInferenceAdapterType);
            var resultIA = await _adapter.SearchAndAnalyzePluginAsync(nameCollection, searchText, embeddingSearchText);
            return SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>.Ok(resultIA);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred in SearchAndAnalyzePluginAsync at: {Message} at: {Time}", ex.Message, DateTime.UtcNow);

#pragma warning disable S6776
            return SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<HotelVector[]>.Error(
                [new SmartCoreHub.Core.SDK.Domain.DTOs.Common.ErrorResponse { Message = ex.Message }],
                ex.Message);
#pragma warning restore S6776
        }
    }

    /// <summary>
    /// Exclui um registro vetorial da coleção pelo seu identificador.
    /// </summary>
    public async Task DeleteAsync(long dataKey)
    {
        await _adapter.DeleteAsync(nameCollection, dataKey);
    }
}
