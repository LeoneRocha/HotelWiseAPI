using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de reservas <see cref="Reservation"/> no MySQL.
/// </summary>
public class ReservationRepository : GenericRepository<Reservation, HotelWiseDbContextMysql>, IReservationRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="ReservationRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public ReservationRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options) { }

    /// <inheritdoc />
    public async Task<Reservation[]> GetByRoomId(long roomId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(r => r.RoomId == roomId)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<Reservation[]> GetReservationsByRoomIdAsync(long roomId)
    {
        return await _context.Reservations
            .Where(r => r.RoomId == roomId)
            .Include(r => r.Room)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<Reservation[]> GetReservationsWithinDateRange(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
            .ToArrayAsync();
    }
}
