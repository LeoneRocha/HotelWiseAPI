using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service
{
    public interface IReservationService : IGenericService<ReservationDto>
    {
        Task<ServiceResponse<string>> CancelReservationAsync(long reservationId); // Cancelamento de uma reserva
        Task<ServiceResponse<ReservationDto>> GetReservationByIdAsync(long reservationId); // Recupera a reserva pelo ID
        Task<ServiceResponse<ReservationDto[]>> GetReservationsByRoomIdAsync(long roomId); // Recupera todas as reservas para um quarto específico

    }
}