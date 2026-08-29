using System.Collections.Concurrent;
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Services;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Services;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Service.Entity.HotelServices;

/// <summary>
/// Serviço de domínio para gestão do cadastro de hotéis, sincronização com base vetorial e extração de tags agregadas.
/// </summary>
public class HotelService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelService
{
    private readonly IGenerateHotelService _generateHotelService;
    private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService;
    private readonly IHotelRepository _hotelRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelService"/> com repositório, mapeadores, validadores e serviços de vetor/geração.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="mapper">Mapeador de objetos AutoMapper.</param>
    /// <param name="applicationConfig">Configuração global de IA.</param>
    /// <param name="hotelRepository">Repositório de dados de hotéis.</param>
    /// <param name="generateHotelService">Serviço de geração sintética de hotéis.</param>
    /// <param name="hotelVectorStoreService">Serviço de armazenamento vetorial.</param>
    /// <param name="entityValidator">Validador FluentValidation de Hotel.</param>
    public HotelService(
        Serilog.ILogger logger,
        IMapper mapper,
        IApplicationIAConfig applicationConfig,
        IHotelRepository hotelRepository,
        IGenerateHotelService generateHotelService,
        IVectorStoreService<HotelVector> hotelVectorStoreService,
        IValidator<Hotel> entityValidator)
        : base(hotelRepository, mapper, logger, entityValidator)
    {
        _generateHotelService = generateHotelService;
        _hotelVectorStoreService = hotelVectorStoreService;
        _hotelRepository = hotelRepository;
    }

    /// <summary>
    /// Obtém todos os hotéis cadastrados ordenados alfabeticamente pelo nome comercial.
    /// </summary>
    /// <returns>Resposta contendo o array de <see cref="HotelDto"/>.</returns>
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
            _logger.Error(ex, "GetAllHotelsAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Indexa ou atualiza o hotel correspondente na base vetorial (Vector Store) para permitir consultas semânticas.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Resposta com indicador de sucesso.</returns>
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
            _logger.Error(ex, "InsertHotelInVectorStore: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Obtém os dados detalhados de um hotel pelo identificador, incluindo o status de indexação vetorial.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Resposta contendo o DTO do hotel.</returns>
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
            _logger.Error(ex, "GetHotelByIdAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Gera um hotel sintético via inteligência artificial com características realistas.
    /// </summary>
    /// <returns>Resposta contendo o DTO do hotel gerado.</returns>
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
            _logger.Error(ex, "GenerateHotelByIA: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Adiciona um novo hotel no banco de dados e sincroniza sua representação vetorial no Vector Store.
    /// </summary>
    /// <param name="hotelDto">DTO do hotel a cadastrar.</param>
    /// <returns>Resposta indicando o sucesso do cadastro.</returns>
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

            // Add Vector
            await addOrUpdateDataVector(hotelDto);

            response.Success = true;
            response.Data = true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "AddHotelAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Atualiza os dados cadastrais do hotel e recalcula o embedding na base vetorial.
    /// </summary>
    /// <param name="hotelDto">DTO com as informações atualizadas.</param>
    /// <returns>Resposta indicando o sucesso da atualização.</returns>
    public async Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotelDto)
    {
        ServiceResponse<bool> response = new ServiceResponse<bool>();
        try
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);

            // Padronizar tags
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
            _logger.Error(ex, "UpdateHotelAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Recupera todas as tags distintas cadastradas entre todos os hotéis do sistema.
    /// </summary>
    /// <returns>Array contendo os nomes das tags em letras minúsculas.</returns>
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
            _logger.Error(ex, "FetchHotelsAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
        }
        return tagsResult.ToArray();
    }

    /// <summary>
    /// Exclui o hotel da base relacional e remove o vetor correspondente do Vector Store.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Resposta indicando o sucesso da exclusão.</returns>
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
            _logger.Error(ex, "DeleteHotelAsync: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            response.Success = false;
            response.Errors.Add(new ErrorResponse() { Message = ex.Message });
        }
        return response;
    }

    /// <summary>
    /// Trata e normaliza o array de tags antes da persistência no banco.
    /// </summary>
    private static void handleTagsBeforeSave(Hotel hotel)
    {
        hotel.Tags = hotel.Tags.Select(t => t.ToLower().Trim()).ToArray();
    }

    /// <summary>
    /// Converte o DTO para <see cref="HotelVector"/> e envia para o Vector Store.
    /// </summary>
    private async Task addOrUpdateDataVector(HotelDto hotelDto)
    {
        if (hotelDto != null)
        {
            try
            {
                await _hotelVectorStoreService.UpsertDataAsync(convertHotelToVector(hotelDto));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "addOrUpdateDataVector: {Message} at: {Time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
        }
    }

    /// <summary>
    /// Converte o DTO de hotel em objeto de dados vetoriais para indexação.
    /// </summary>
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