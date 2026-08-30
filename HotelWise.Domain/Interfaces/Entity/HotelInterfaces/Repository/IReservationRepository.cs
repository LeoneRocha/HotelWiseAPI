using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;

/// <summary>
/// Contrato de repositório específico para consultas e operações de persistência da entidade <see cref="Reservation"/>.
/// </summary>
public interface IReservationRepository : IGenericRepository<Reservation>
{
    /// <summary>
    /// Obtém todas as reservas associadas a um determinado quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de reservas vinculadas ao quarto.</returns>
    Task<Reservation[]> GetByRoomId(long roomId);

    /// <summary>
    /// Obtém de forma assíncrona todas as reservas associadas a um quarto específico.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de reservas encontradas.</returns>
    Task<Reservation[]> GetReservationsByRoomIdAsync(long roomId);

    /// <summary>
    /// Obtém as reservas que possuem sobreposição com o intervalo de datas informado.
    /// </summary>
    /// <param name="startDate">Data inicial do intervalo de pesquisa.</param>
    /// <param name="endDate">Data final do intervalo de pesquisa.</param>
    /// <returns>Array de reservas ativas no período.</returns>
    Task<Reservation[]> GetReservationsWithinDateRange(DateTime startDate, DateTime endDate);
}

