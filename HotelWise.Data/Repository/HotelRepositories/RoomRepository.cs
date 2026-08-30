using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de quartos <see cref="Room"/> com relacionamentos de Hotel e disponibilidades no MySQL.
/// </summary>
public class RoomRepository : GenericRepositoryBase<Room, HotelWiseDbContextMysql>, IRoomRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public RoomRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, options) { }

    /// <summary>
    /// Localiza um quarto pelo identificador desabilitando o rastreamento de mudanças e incluindo os dados do hotel associado.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Entidade <see cref="Room"/> encontrada ou <c>null</c> se inexistente.</returns>
    public async Task<Room?> FindByRoomIdAsNoTracking(long roomId)
    {
        return await _dataset
            .Include(r => r.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    /// <summary>
    /// Recupera todos os quartos associados a um hotel específico incluindo as disponibilidades cadastradas.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Array de quartos com suas disponibilidades.</returns>
    public async Task<Room[]> GetRoomsByHotelIdAsync(long hotelId)
    {
        return await _context.Rooms
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.RoomAvailabilities)
            .ToArrayAsync();
    }

    /// <summary>
    /// Obtém todos os quartos de um determinado hotel sem rastreamento de mudanças.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Array de quartos do hotel.</returns>
    public async Task<Room[]> GetRoomsByHotelAsNoTracking(long hotelId)
    {
        return await _dataset
            .AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .ToArrayAsync();
    }
}

