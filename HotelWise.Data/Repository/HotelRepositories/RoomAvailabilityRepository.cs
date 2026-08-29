using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Data.Context;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository.HotelRepositories;

/// <summary>
/// Implementação concreta do repositório de disponibilidades de quartos <see cref="RoomAvailability"/> no MySQL.
/// </summary>
public class RoomAvailabilityRepository : GenericRepositoryBase<RoomAvailability, HotelWiseDbContextMysql>, IRoomAvailabilityRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomAvailabilityRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public RoomAvailabilityRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, options) { }

    /// <summary>
    /// Retorna as disponibilidades cadastradas para um quarto sem rastreamento de entidades.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array de registros de disponibilidade do quarto.</returns>
    public async Task<RoomAvailability[]> GetAvailabilityByRoomId(long roomId)
    {
        return await _dataset
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId)
            .ToArrayAsync();
    }

    /// <summary>
    /// Retorna as disponibilidades de um quarto que interceptam o intervalo de datas especificado.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <param name="startDate">Data inicial do período.</param>
    /// <param name="endDate">Data final do período.</param>
    /// <returns>Array de disponibilidades vigentes no período.</returns>
    public async Task<RoomAvailability[]> GetAvailabilityByDateRange(long roomId, DateTime startDate, DateTime endDate)
    {
        return await _dataset
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId &&
                         ra.StartDate <= endDate &&
                         ra.EndDate >= startDate)
            .ToArrayAsync();
    }

    /// <summary>
    /// Busca disponibilidades para todos os quartos de um hotel dentro de um período e com uma moeda de cotação específica.
    /// </summary>
    /// <param name="request">DTO contendo o identificador do hotel, datas e moeda.</param>
    /// <returns>Array de disponibilidades de quartos encontradas.</returns>
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
