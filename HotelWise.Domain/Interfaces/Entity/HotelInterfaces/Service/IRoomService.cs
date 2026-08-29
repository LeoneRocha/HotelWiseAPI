using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço para gerenciamento e consultas dos quartos dos estabelecimentos hoteleiros.
/// </summary>
public interface IRoomService : IGenericService<RoomDto>
{
    /// <summary>
    /// Recupera todos os quartos pertencentes a um determinado hotel.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Resposta contendo o array de quartos do hotel.</returns>
    Task<ServiceResponse<RoomDto[]>> GetRoomsByHotelIdAsync(long hotelId);
}
