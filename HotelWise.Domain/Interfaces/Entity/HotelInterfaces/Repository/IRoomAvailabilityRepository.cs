using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;

/// <summary>
/// Contrato de repositório específico para consultas e operações de persistência da entidade <see cref="RoomAvailability"/>.
/// </summary>
public interface IRoomAvailabilityRepository : IGenericRepository<RoomAvailability>
{
    /// <summary>
    /// Retorna as disponibilidades cadastradas para um determinado quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de disponibilidades do quarto.</returns>
    Task<RoomAvailability[]> GetAvailabilityByRoomId(long roomId);

    /// <summary>
    /// Retorna as disponibilidades de um quarto dentro de um intervalo específico de datas.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <param name="startDate">Data inicial do período.</param>
    /// <param name="endDate">Data final do período.</param>
    /// <returns>Array de disponibilidades vigentes no período informado.</returns>
    Task<RoomAvailability[]> GetAvailabilityByDateRange(long roomId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Recupera todas as disponibilidades de quartos para um determinado hotel dentro de um período especificado.
    /// </summary>
    /// <param name="request">DTO contendo o identificador do hotel e as datas de início e fim da busca.</param>
    /// <returns>Array de disponibilidades dos quartos do hotel no período.</returns>
    Task<RoomAvailability[]> GetAvailabilitiesByHotelAndPeriodAsync(HotelAvailabilityRequestDto request);
}

