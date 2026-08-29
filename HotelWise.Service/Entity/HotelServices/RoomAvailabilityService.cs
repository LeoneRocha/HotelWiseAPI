using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Services;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using Serilog;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de aplicação para gerenciamento de disponibilidades, processamento em lote e consultas de ocupação por período.
/// </summary>
public class RoomAvailabilityService : GenericEntityServiceBase<RoomAvailability, RoomAvailabilityDto>, IRoomAvailabilityService
{
    private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
    private readonly IRoomRepository _roomRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomAvailabilityService"/>.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="repository">Repositório de disponibilidades.</param>
    /// <param name="roomRepository">Repositório de quartos.</param>
    /// <param name="mapper">Mapeador de objetos AutoMapper.</param>
    /// <param name="entityValidator">Validador FluentValidation de disponibilidades.</param>
    public RoomAvailabilityService(
        ILogger logger,
        IRoomAvailabilityRepository repository,
        IRoomRepository roomRepository,
        IMapper mapper,
        IValidator<RoomAvailability> entityValidator
    ) : base(repository, mapper, logger, entityValidator)
    {
        _roomAvailabilityRepository = repository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Cria uma nova disponibilidade de quarto após validação rigorosa de regras de negócio.
    /// </summary>
    /// <param name="availabilityDto">DTO da disponibilidade a ser criada.</param>
    /// <returns>Resposta contendo o DTO da disponibilidade criada.</returns>
    public override async Task<ServiceResponse<RoomAvailabilityDto>> CreateAsync(RoomAvailabilityDto availabilityDto)
    {
        var response = new ServiceResponse<RoomAvailabilityDto>();

        // Mapeia o DTO para a entidade
        var roomAvailability = _mapper.Map<RoomAvailability>(availabilityDto);

        // Valida a disponibilidade antes de criar
        var validationResult = await _entityValidator.ValidateAsync(roomAvailability);
        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return response;
        }

        // Insere a disponibilidade no banco
        var createdAvailability = await _repository.AddAsync(roomAvailability);

        // Retorna a disponibilidade criada no formato DTO
        response.Data = _mapper.Map<RoomAvailabilityDto>(createdAvailability);
        response.Success = true;
        response.Message = "Disponibilidade criada com sucesso.";
        return response;
    }

    /// <summary>
    /// Cria ou atualiza múltiplas disponibilidades em uma operação em lote unificada.
    /// </summary>
    /// <param name="availabilitiesDto">Array de DTOs contendo itens para criação (Id = 0) e atualização (Id > 0).</param>
    /// <returns>Resposta com o relatório da operação em lote.</returns>
    public async Task<ServiceResponse<string>> CreateBatchAsync(RoomAvailabilityDto[] availabilitiesDto)
    {
        try
        {
            var (itemsToCreate, itemsToUpdate) = SeparateItems(availabilitiesDto);

            if (itemsToCreate.Length == 0 && itemsToUpdate.Length == 0) return ResponseBuilder<string>.BuildError("Nenhum item para criar e atulizar");

            // Processar criações e atualizações
            var createResult = await ProcessCreationsAsync(itemsToCreate);
            if (!createResult.Success) return createResult;

            var updateResult = await ProcessUpdatesAsync(itemsToUpdate);
            if (!updateResult.Success) return updateResult;

            // Construir mensagem de sucesso
            return ResponseBuilder<string>.BuildSuccess(
                $"Operação em lote concluída com sucesso: {itemsToCreate.Length} itens criados e {itemsToUpdate.Length} itens atualizados."
            );
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao processar operação em lote: {Message}", ex.Message);
            return ResponseBuilder<string>.BuildError($"Erro ao processar operação em lote: {ex.Message}");
        }
    }

    /// <summary>
    /// Separa itens para criação e atualização com base no valor do identificador.
    /// </summary>
    private static (RoomAvailabilityDto[] itemsToCreate, RoomAvailabilityDto[] itemsToUpdate) SeparateItems(RoomAvailabilityDto[] availabilitiesDto)
    {
        return (availabilitiesDto.Where(a => a.Id == 0).ToArray(), availabilitiesDto.Where(a => a.Id > 0).ToArray());
    }

    /// <summary>
    /// Processa a criação em lote de disponibilidades validando cada item.
    /// </summary>
    private async Task<ServiceResponse<string>> ProcessCreationsAsync(RoomAvailabilityDto[] itemsToCreate)
    {
        var newAvailabilities = _mapper.Map<RoomAvailability[]>(itemsToCreate);

        foreach (var availability in newAvailabilities)
        {
            var validationResult = await ValidateAvailabilityAsync(availability);
            if (!validationResult.IsValid)
            {
                return ResponseBuilder<string>.BuildError($"Erro ao criar item: {FormatValidationErrors(validationResult)}");
            }
        }
        await _repository.AddRangeAsync(newAvailabilities);
        return ResponseBuilder<string>.BuildSuccess($"{itemsToCreate.Length} itens criados com sucesso");
    }

    /// <summary>
    /// Processa a atualização em lote de disponibilidades validando cada registro existente.
    /// </summary>
    private async Task<ServiceResponse<string>> ProcessUpdatesAsync(RoomAvailabilityDto[] itemsToUpdate)
    {
        List<RoomAvailability> roomAvailabilitiesUpdates = new List<RoomAvailability>();

        foreach (var availabilityDto in itemsToUpdate)
        {
            var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(availabilityDto.Id);
            if (existingAvailability == null)
            {
                return ResponseBuilder<string>.BuildError($"Disponibilidade com ID {availabilityDto.Id} não encontrada");
            }
            existingAvailability.AvailabilityWithPrice = availabilityDto.AvailabilityWithPrice;
            var validationResult = await ValidateAvailabilityAsync(existingAvailability);
            if (!validationResult.IsValid)
            {
                return ResponseBuilder<string>.BuildError(
                    $"Erro ao atualizar item {existingAvailability.Id}: {FormatValidationErrors(validationResult)}"
                );
            }
            roomAvailabilitiesUpdates.Add(existingAvailability);
        }
        await _repository.UpdateRangeAsync(roomAvailabilitiesUpdates);

        return ResponseBuilder<string>.BuildSuccess($"{itemsToUpdate.Length} itens atualizados com sucesso");
    }

    /// <summary>
    /// Valida uma disponibilidade usando o validador FluentValidation configurado.
    /// </summary>
    private async Task<FluentValidation.Results.ValidationResult> ValidateAvailabilityAsync(RoomAvailability availability)
    {
        return await _entityValidator.ValidateAsync(availability);
    }

    /// <summary>
    /// Formata as falhas de validação em uma string consolidada.
    /// </summary>
    private static string FormatValidationErrors(FluentValidation.Results.ValidationResult validationResult)
    {
        return string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
    }

    /// <summary>
    /// Classe utilitária para construção rápida de instâncias de <see cref="ServiceResponse{T}"/>.
    /// </summary>
    /// <typeparam name="T">Tipo do payload de dados.</typeparam>
    public static class ResponseBuilder<T>
    {
        /// <summary>
        /// Constrói uma resposta de sucesso com mensagem.
        /// </summary>
        public static ServiceResponse<T> BuildSuccess(string message) =>
            new ServiceResponse<T> { Success = true, Message = message };

        /// <summary>
        /// Constrói uma resposta de erro com mensagem.
        /// </summary>
        public static ServiceResponse<T> BuildError(string message) =>
            new ServiceResponse<T> { Success = false, Message = message };
    }

    /// <summary>
    /// Atualiza uma disponibilidade de quarto existente após revalidação.
    /// </summary>
    /// <param name="availabilityDto">DTO com os dados atualizados.</param>
    /// <returns>Resposta contendo o DTO atualizado.</returns>
    public override async Task<ServiceResponse<RoomAvailabilityDto>> UpdateAsync(RoomAvailabilityDto availabilityDto)
    {
        var response = new ServiceResponse<RoomAvailabilityDto>();
        var availabilityId = availabilityDto.Id;

        // Busca a disponibilidade pelo ID
        var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(availabilityId);
        if (existingAvailability == null)
        {
            response.Success = false;
            response.Message = "Disponibilidade não encontrada.";
            return response;
        }

        // Atualiza os dados da disponibilidade
        var roomAvailability = _mapper.Map<RoomAvailability>(availabilityDto);
        roomAvailability.Id = availabilityId;

        // Valida a disponibilidade antes de atualizar
        var validationResult = await _entityValidator.ValidateAsync(roomAvailability);
        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return response;
        }

        // Atualiza no banco
        var updatedAvailability = await _repository.UpdateAsync(roomAvailability);

        // Retorna a disponibilidade atualizada no formato DTO
        response.Data = _mapper.Map<RoomAvailabilityDto>(updatedAvailability);
        response.Success = true;
        response.Message = "Disponibilidade atualizada com sucesso.";
        return response;
    }

    /// <summary>
    /// Exclui uma disponibilidade de quarto pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador único da disponibilidade.</param>
    /// <returns>Resposta indicando a exclusão.</returns>
    public override async Task<ServiceResponse<string>> DeleteAsync(long id)
    {
        var response = new ServiceResponse<string>();

        // Busca a disponibilidade pelo ID
        var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(id);
        if (existingAvailability == null)
        {
            response.Success = false;
            response.Message = "Disponibilidade não encontrada.";
            return response;
        }

        // Exclui a disponibilidade
        await _repository.DeleteAsync(id);

        response.Success = true;
        response.Message = "Disponibilidade excluída com sucesso.";
        return response;
    }

    /// <summary>
    /// Recupera todas as disponibilidades associadas a um quarto específico.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Resposta contendo o array de disponibilidades do quarto.</returns>
    public async Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesByRoomIdAsync(long roomId)
    {
        var response = new ServiceResponse<RoomAvailabilityDto[]>();

        // Verifica se o quarto existe
        var roomExists = await _roomRepository.ExistsAsync(r => r.Id == roomId);
        if (!roomExists)
        {
            response.Success = false;
            response.Message = "O quarto informado não existe.";
            return response;
        }

        // Busca todas as disponibilidades pelo RoomId
        var availabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(roomId);

        // Retorna as disponibilidades no formato DTO
        response.Data = _mapper.Map<RoomAvailabilityDto[]>(availabilities);
        response.Success = true;
        response.Message = "Disponibilidades recuperadas com sucesso.";
        return response;
    }

    /// <summary>
    /// Lista as disponibilidades com base no hotel e no período opcional informado.
    /// </summary>
    /// <param name="searchDto">Parâmetros de busca encapsulados em DTO.</param>
    /// <returns>Array de RoomAvailabilityDto encapsulado em ServiceResponse.</returns>
    public async Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesBySearchCriteriaAsync(RoomAvailabilitySearchDto searchDto)
    {
        var response = new ServiceResponse<RoomAvailabilityDto[]>();

        // Busca diretamente no repositório as disponibilidades com base nos critérios informados
        var availabilities = await _roomAvailabilityRepository.GetAvailabilitiesByHotelAndPeriodAsync(new HotelAvailabilityRequestDto()
        {
            Currency = searchDto.Currency,
            HotelId = searchDto.HotelId,
            StartDate = searchDto.StartDate,
            EndDate = searchDto.EndDate
        });

        // Retorna as disponibilidades no formato DTO
        response.Data = _mapper.Map<RoomAvailabilityDto[]>(availabilities);
        response.Success = true;
        response.Message = "Disponibilidades recuperadas com sucesso.";
        return response;
    }
}
