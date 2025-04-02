﻿using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces
{
    public interface IReservationService
    {
        Task<ServiceResponse<ReservationDto>> CreateReservationAsync(ReservationDto reservationDto); // Criação de uma nova reserva
        Task<ServiceResponse<string>> CancelReservationAsync(long reservationId); // Cancelamento de uma reserva
        Task<ServiceResponse<ReservationDto>> GetReservationByIdAsync(long reservationId); // Recupera a reserva pelo ID
        Task<ServiceResponse<IEnumerable<ReservationDto>>> GetReservationsByRoomIdAsync(long roomId); // Recupera todas as reservas para um quarto específico
    }
}