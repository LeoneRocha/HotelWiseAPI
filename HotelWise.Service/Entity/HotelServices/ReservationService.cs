using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Generic;
using Serilog;

namespace HotelWise.Service.Entity
{
    public class ReservationService : GenericEntityServiceBase<Reservation, ReservationDto>, IReservationService
    {
        private readonly IRoomRepository _roomRepository; 
        private readonly IReservationRepository _reservationRepository;

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
        public override Task<ServiceResponse<ReservationDto>> UpdateAsync(ReservationDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cria uma nova reserva após validação de negócios.
        /// </summary>
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
        /// Cancela uma reserva existente, verificando as regras de cancelamento.
        /// </summary>
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
        /// Busca a reserva pelo ID.
        /// </summary>
        public async Task<ServiceResponse<ReservationDto>> GetReservationByIdAsync(long reservationId)
        {
            var response = new ServiceResponse<ReservationDto>();

            // Busca a reserva pelo ID
            var reservation = await _repository.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                response.Success = false;
                response.Message = "Reserva não encontrada.";
                return response;
            }

            // Retorna a reserva encontrada no formato DTO
            response.Data = _mapper.Map<ReservationDto>(reservation);
            response.Success = true;
            response.Message = "Reserva encontrada com sucesso.";
            return response;
        }

        /// <summary>
        /// Busca todas as reservas de um quarto específico.
        /// </summary>
        public async Task<ServiceResponse<ReservationDto[]>> GetReservationsByRoomIdAsync(long roomId)
        {
            var response = new ServiceResponse<ReservationDto[]>();

            // Verifica se o quarto existe
            var roomExists = await _roomRepository.ExistsAsync(r => r.Id == roomId);
            if (!roomExists)
            {
                response.Success = false;
                response.Message = "Quarto informado não existe.";
                return response;
            }

            // Busca todas as reservas associadas ao quarto
            var reservations = await _reservationRepository.GetReservationsByRoomIdAsync(roomId);

            // Retorna as reservas no formato DTO
            response.Data = _mapper.Map<ReservationDto[]>(reservations);
            response.Success = true;
            response.Message = "Reservas recuperadas com sucesso.";
            return response;
        }


    }
}
