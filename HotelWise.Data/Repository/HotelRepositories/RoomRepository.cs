using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de quartos <see cref="Room"/> com relacionamentos de Hotel e disponibilidades no MySQL.
/// </summary>
public class RoomRepository : GenericRepository<Room, HotelWiseDbContextMysql>, IRoomRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public RoomRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options) { }

    /// <inheritdoc />
    public async Task<Room?> FindByRoomIdAsNoTracking(long roomId)
    {
        return await _dbSet
            .Include(r => r.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    /// <inheritdoc />
    public async Task<Room[]> GetRoomsByHotelIdAsync(long hotelId)
    {
        return await _context.Rooms
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.RoomAvailabilities)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<Room[]> GetRoomsByHotelAsNoTracking(long hotelId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .ToArrayAsync();
    }
}
