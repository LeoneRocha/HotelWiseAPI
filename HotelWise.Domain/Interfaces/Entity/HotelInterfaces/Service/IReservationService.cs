using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço para gerenciamento do ciclo de vida, consultas e cancelamento de reservas hoteleiras.
/// </summary>
public interface IReservationService : IGenericService<ReservationDto>
{
    /// <summary>
    /// Cancela uma reserva existente de acordo com as regras de negócio de antecedência.
    /// </summary>
    /// <param name="reservationId">Identificador único da reserva a ser cancelada.</param>
    /// <returns>Resposta contendo mensagem do status do cancelamento.</returns>
    Task<ServiceResponse<string>> CancelReservationAsync(long reservationId);

    /// <summary>
    /// Recupera os dados completos de uma reserva pelo identificador.
    /// </summary>
    /// <param name="reservationId">Identificador único da reserva.</param>
    /// <returns>Resposta contendo os detalhes da reserva.</returns>
    Task<ServiceResponse<ReservationDto>> GetReservationByIdAsync(long reservationId);

    /// <summary>
    /// Recupera todas as reservas registradas para um determinado quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Resposta contendo o array de reservas do quarto.</returns>
    Task<ServiceResponse<ReservationDto[]>> GetReservationsByRoomIdAsync(long roomId);
}