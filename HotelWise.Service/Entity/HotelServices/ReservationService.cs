using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using Serilog;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de domínio para criação, validação de regras de estadia, cancelamento e consulta de reservas hoteleiras.
/// </summary>
public class ReservationService : GenericEntityServiceBase<Reservation, ReservationDto>, IReservationService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IReservationRepository _reservationRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ReservationService"/> com os repositórios de reservas e quartos, logger, mapper e validador.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="repository">Repositório de reservas.</param>
    /// <param name="roomRepository">Repositório de quartos.</param>
    /// <param name="mapper">Mapeador AutoMapper.</param>
    /// <param name="entityValidator">Validador FluentValidation de reservas.</param>
    public ReservationService(
          ILogger logger,
          IReservationRepository repository,
          IRoomRepository roomRepository,
          IMapper mapper,
          IValidator<Reservation> entityValidator
    ) : base(repository, mapper, logger, entityValidator)
    {
        _roomRepository = roomRepository;
        _reservationRepository = repository;
    }

    /// <summary>
    /// Operação de atualização direta não suportada para reservas (utilize métodos de ciclo de vida específicos).
    /// </summary>
    /// <param name="entityDto">DTO da entidade.</param>
    /// <exception cref="NotImplementedException">Sempre lançada.</exception>
    public override Task<ServiceResponse<ReservationDto>> UpdateAsync(ReservationDto entityDto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Operação de exclusão direta não suportada para reservas (utilize cancelamento).
    /// </summary>
    /// <param name="id">Identificador da reserva.</param>
    /// <exception cref="NotImplementedException">Sempre lançada.</exception>
    public override Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cria uma nova reserva após validação rigorosa de regras de negócio, disponibilidade e antecedência.
    /// </summary>
    /// <param name="reservationDto">DTO com os dados da reserva a ser criada.</param>
    /// <returns>Resposta contendo o DTO da reserva cadastrada.</returns>
    public override async Task<ServiceResponse<ReservationDto>> CreateAsync(ReservationDto reservationDto)
    {
        var response = new ServiceResponse<ReservationDto>();

        // Mapeia o DTO para a entidade
        var reservation = _mapper.Map<Reservation>(reservationDto);

        // Valida a reserva antes de criar
        var validationResult = await _entityValidator.ValidateAsync(reservation);
        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return response;
        }

        // Insere a reserva no banco
        var createdReservation = await _repository.AddAsync(reservation);

        // Retorna a reserva criada no formato DTO
        response.Data = _mapper.Map<ReservationDto>(createdReservation);
        response.Success = true;
        response.Message = "Reserva criada com sucesso.";
        return response;
    }

    /// <summary>
    /// Cancela uma reserva existente verificando as regras de antecedência mínima de dias úteis.
    /// </summary>
    /// <param name="reservationId">Identificador único da reserva.</param>
    /// <returns>Resposta com status e mensagem do cancelamento.</returns>
    public async Task<ServiceResponse<string>> CancelReservationAsync(long reservationId)
    {
        var response = new ServiceResponse<string>();

        // Busca a reserva pelo ID
        var reservation = await _repository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            response.Success = false;
            response.Message = "Reserva não encontrada.";
            return response;
        }

        // Altera o status para cancelado e valida novamente
        reservation.Status = ReservationStatus.Cancelled;

        var validationResult = await _entityValidator.ValidateAsync(reservation);
        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return response;
        }

        // Atualiza a reserva no banco
        await _repository.UpdateAsync(reservation);

        response.Success = true;
        response.Message = "Reserva cancelada com sucesso.";
        return response;
    }

    /// <summary>
    /// Busca uma reserva pelo seu identificador primário.
    /// </summary>
    /// <param name="reservationId">Identificador único da reserva.</param>
    /// <returns>Resposta contendo o DTO da reserva encontrada.</returns>
    public async Task<ServiceResponse<ReservationDto>> GetReservationByIdAsync(long reservationId)
    {
        var response = new ServiceResponse<ReservationDto>();

        var reservation = await _repository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            response.Success = false;
            response.Message = "Reserva não encontrada.";
            return response;
        }

        response.Data = _mapper.Map<ReservationDto>(reservation);
        response.Success = true;
        response.Message = "Reserva encontrada com sucesso.";
        return response;
    }

    /// <summary>
    /// Busca todas as reservas cadastradas para um determinado quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Resposta contendo o array de reservas do quarto.</returns>
    public async Task<ServiceResponse<ReservationDto[]>> GetReservationsByRoomIdAsync(long roomId)
    {
        var response = new ServiceResponse<ReservationDto[]>();

        var roomExists = await _roomRepository.ExistsAsync(r => r.Id == roomId);
        if (!roomExists)
        {
            response.Success = false;
            response.Message = "Quarto informado não existe.";
            return response;
        }

        var reservations = await _reservationRepository.GetReservationsByRoomIdAsync(roomId);

        response.Data = _mapper.Map<ReservationDto[]>(reservations);
        response.Success = true;
        response.Message = "Reservas recuperadas com sucesso.";
        return response;
    }
}

