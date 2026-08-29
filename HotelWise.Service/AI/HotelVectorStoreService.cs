using AutoMapper;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;
using HotelWise.Core.SDK.AI.Services;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Domain.Dto.IA.SemanticKernel;

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

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelVectorStoreService"/> com os adaptadores de inferência e de armazenamento vetorial.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="mapper">Mapeador de objetos AutoMapper.</param>
    /// <param name="applicationIAConfig">Configuração de IA e Vector Store.</param>
    /// <param name="adapterFactory">Fábrica de adaptadores de Vector Store.</param>
    /// <param name="aIInferenceService">Serviço de inferência de IA.</param>
    public HotelVectorStoreService(
        Serilog.ILogger logger,
        IMapper mapper,
        IApplicationIAConfig applicationIAConfig,
        IVectorStoreAdapterFactory adapterFactory,
        IAIInferenceService aIInferenceService) : base(mapper, logger)
    {
        _eIAInferenceAdapterType = applicationIAConfig.RagConfig.GetAInferenceAdapterType();
        _adapter = adapterFactory.CreateAdapter<HotelVector>();
        _aIInferenceService = aIInferenceService;

        nameCollection = $"{applicationIAConfig.RagConfig.VectorStoreCollectionPrefixName}skhotels";
    }

    /// <summary>
    /// Gera embeddings vetoriais para o texto informado.
    /// </summary>
    /// <param name="text">Texto a ser vetorizado.</param>
    /// <returns>Array de float com o vetor gerado, ou <c>null</c> em caso de erro.</returns>
    public async Task<float[]?> GenerateEmbeddingAsync(string text)
    {
        return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
    }

    /// <summary>
    /// Obtém um registro vetorial de hotel a partir de sua chave identificadora.
    /// </summary>
    /// <param name="dataKey">Chave do registro vetorial.</param>
    /// <returns>Registro <see cref="HotelVector"/> ou <c>null</c> se não encontrado.</returns>
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
    /// <param name="entity">Registro vetorial a ser persistido.</param>
    public async Task UpsertDataAsync(HotelVector entity)
    {
        var embedding = await _aIInferenceService.GenerateEmbeddingAsync(entity.Description, _eIAInferenceAdapterType);

        entity.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);

        await _adapter.UpsertDataAsync(nameCollection, entity);
    }

    /// <summary>
    /// Insere ou atualiza múltiplos registros vetoriais de hotéis em lote.
    /// </summary>
    /// <param name="listEntity">Coleção de vetores a persistir.</param>
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
    /// <param name="searchCriteria">Critérios contendo a consulta e limites.</param>
    /// <returns>Resposta contendo o array de registros <see cref="HotelVector"/> encontrados.</returns>
    public async Task<ServiceResponse<HotelVector[]>> VectorizedSearchAsync(SearchCriteria searchCriteria)
    {
        ServiceResponse<HotelVector[]> response = new ServiceResponse<HotelVector[]>();
        try
        {
            //Get semantic search 
            var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchCriteria.SearchTextCriteria, _eIAInferenceAdapterType);

            var hotelsVector = await _adapter.VectorizedSearchAsync(nameCollection, embeddingSearchText, searchCriteria);
            response.Success = true;
            response.Data = hotelsVector;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred in VectorizedSearchAsync at: {Message} at: {Time}", ex.Message, DateTime.UtcNow);

#pragma warning disable S6776
            response.Success = false;
            response.Message = ex.Message;
            // NOSONAR
            response.Errors = new List<ErrorResponse>() { new ErrorResponse() { Message = ex.Message } };
#pragma warning restore S6776
        }
        return response;
    }

    /// <summary>
    /// Executa busca e análise combinada com plugins do Semantic Kernel para interpretação de intenção de busca.
    /// </summary>
    /// <param name="searchText">Texto da consulta do usuário.</param>
    /// <returns>Resposta contendo os registros vetoriais filtrados.</returns>
    public async Task<ServiceResponse<HotelVector[]>> SearchAndAnalyzePluginAsync(string searchText)
    {
        ServiceResponse<HotelVector[]> response = new ServiceResponse<HotelVector[]>();
        try
        {
            //Get semantic search 
            var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchText, _eIAInferenceAdapterType);

            var resultIA = await _adapter.SearchAndAnalyzePluginAsync(nameCollection, searchText, embeddingSearchText);

            response.Success = true;
            response.Data = resultIA;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred in SearchAndAnalyzePluginAsync at: {Message} at: {Time}", ex.Message, DateTime.UtcNow);

            response.Success = false;
            response.Message = ex.Message;

#pragma warning disable S6776
            // NOSONAR
            response.Errors = new List<ErrorResponse>() { new ErrorResponse() { Message = ex.Message } };
#pragma warning restore S6776
        }
        return response;
    }

    /// <summary>
    /// Exclui um registro vetorial da coleção pelo seu identificador.
    /// </summary>
    /// <param name="dataKey">Chave do registro a remover.</param>
    public async Task DeleteAsync(long dataKey)
    {
        await _adapter.DeleteAsync(nameCollection, dataKey);
    } 
}