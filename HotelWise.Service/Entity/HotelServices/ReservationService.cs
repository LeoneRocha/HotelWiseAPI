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
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(
              ILogger logger,
              IReservationRepository repository,
              IRoomRepository roomRepository,
              IRoomAvailabilityRepository roomAvailabilityRepository,
              IMapper mapper,
              IValidator<Reservation> entityValidator
        ) : base(repository, mapper, logger, entityValidator)
        {
            _roomRepository = roomRepository;
            _roomAvailabilityRepository = roomAvailabilityRepository;
            _reservationRepository = repository;
        }

        /// <summary>
        /// Cria uma nova reserva após validação de negócios.
        /// </summary>
        public async Task<ServiceResponse<ReservationDto>> CreateReservationAsync(ReservationDto reservationDto)
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
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                response.Success = false;
                response.Message = "Reserva não encontrada.";
                return response;
            }

            // Verifica se pode ser cancelada
            if (!CanReservationBeCancelled(reservation))
            {
                response.Success = false;
                response.Message = "A reserva só pode ser cancelada com pelo menos 3 dias úteis de antecedência.";
                return response;
            }

            // Altera o status para cancelado e atualiza
            reservation.Status = ReservationStatus.Cancelled;
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
        public async Task<ServiceResponse<IEnumerable<ReservationDto>>> GetReservationsByRoomIdAsync(long roomId)
        {
            var response = new ServiceResponse<IEnumerable<ReservationDto>>();

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
            response.Data = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
            response.Success = true;
            response.Message = "Reservas recuperadas com sucesso.";
            return response;
        }

        /// <summary>
        /// Verifica se uma reserva pode ser cancelada com base na regra de 3 dias úteis.
        /// </summary>
        private static bool CanReservationBeCancelled(Reservation reservation)
        {
            if (reservation.Status == ReservationStatus.Cancelled)
                return false;

            var currentDate = DateTime.UtcNow.Date;
            var businessDays = CalculateBusinessDaysBetween(currentDate, reservation.CheckInDate.Date);
            return businessDays >= 3;
        }

        /// <summary>
        /// Calcula o número de dias úteis entre duas datas.
        /// </summary>
        private static int CalculateBusinessDaysBetween(DateTime start, DateTime end)
        {
            if (end <= start)
                return 0;

            int totalDays = (end - start).Days;
            int businessDays = 0;

            for (int i = 0; i < totalDays; i++)
            {
                var day = start.AddDays(i).DayOfWeek;
                if (day != DayOfWeek.Saturday && day != DayOfWeek.Sunday)
                    businessDays++;
            }

            return businessDays;
        }
    }
}
