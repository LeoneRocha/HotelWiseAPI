using HotelWise.Data.Context;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository.HotelRepositories;

/// <summary>
/// Implementação concreta do repositório de disponibilidades de quartos <see cref="RoomAvailability"/> no MySQL.
/// </summary>
public class RoomAvailabilityRepository : GenericRepository<RoomAvailability, HotelWiseDbContextMysql>, IRoomAvailabilityRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomAvailabilityRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public RoomAvailabilityRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options) { }

    /// <inheritdoc />
    public async Task<RoomAvailability[]> GetAvailabilityByRoomId(long roomId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<RoomAvailability[]> GetAvailabilityByDateRange(long roomId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId &&
                         ra.StartDate <= endDate &&
                         ra.EndDate >= startDate)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<RoomAvailability[]> GetAvailabilitiesByHotelAndPeriodAsync(HotelAvailabilityRequestDto request)
    {
        return await _context.RoomAvailabilities
            .Where(availability =>
                availability.Room.HotelId == request.HotelId &&
                (
                    (availability.StartDate >= request.StartDate && availability.StartDate <= request.EndDate) ||
                    (availability.EndDate >= request.StartDate && availability.EndDate <= request.EndDate) ||
                    (availability.StartDate <= request.StartDate && availability.EndDate >= request.EndDate)
                ) &&
                availability.Currency == request.Currency
            )
            .Include(availability => availability.Room)
            .ToArrayAsync();
    }
}
