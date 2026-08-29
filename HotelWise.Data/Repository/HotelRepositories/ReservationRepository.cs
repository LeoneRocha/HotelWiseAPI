using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de reservas <see cref="Reservation"/> no MySQL.
/// </summary>
public class ReservationRepository : GenericRepositoryBase<Reservation, HotelWiseDbContextMysql>, IReservationRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="ReservationRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public ReservationRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, options) { }

    /// <summary>
    /// Obtém todas as reservas de um determinado quarto sem rastreamento de entidades.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de reservas vinculadas ao quarto.</returns>
    public async Task<Reservation[]> GetByRoomId(long roomId)
    {
        return await _dataset
            .AsNoTracking()
            .Where(r => r.RoomId == roomId)
            .ToArrayAsync();
    }

    /// <summary>
    /// Recupera todas as reservas associadas a um quarto específico incluindo os detalhes do quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de reservas com os dados de quarto carregados.</returns>
    public async Task<Reservation[]> GetReservationsByRoomIdAsync(long roomId)
    {
        return await _context.Reservations
            .Where(r => r.RoomId == roomId)
            .Include(r => r.Room)
            .ToArrayAsync();
    }

    /// <summary>
    /// Obtém as reservas que possuem sobreposição com o intervalo de datas especificado.
    /// </summary>
    /// <param name="startDate">Data inicial do período de pesquisa.</param>
    /// <param name="endDate">Data final do período de pesquisa.</param>
    /// <returns>Array de reservas ativas no período.</returns>
    public async Task<Reservation[]> GetReservationsWithinDateRange(DateTime startDate, DateTime endDate)
    {
        return await _dataset
            .AsNoTracking()
            .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
            .ToArrayAsync();
    }
}