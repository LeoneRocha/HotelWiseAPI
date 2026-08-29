using HotelWise.Core.SDK.Abstractions;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;

/// <summary>
/// Contrato de repositório específico para consultas e persistência da entidade <see cref="Room"/>.
/// </summary>
public interface IRoomRepository : IGenericRepository<Room>
{
    /// <summary>
    /// Localiza um quarto pelo identificador desabilitando o rastreamento de mudanças do EF Core.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Instância de <see cref="Room"/> ou <c>null</c> se não encontrado.</returns>
    Task<Room?> FindByRoomIdAsNoTracking(long roomId);

    /// <summary>
    /// Obtém todos os quartos de um determinado hotel sem rastreamento de entidades.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Array de quartos pertencentes ao hotel.</returns>
    Task<Room[]> GetRoomsByHotelAsNoTracking(long hotelId);

    /// <summary>
    /// Obtém de forma assíncrona todos os quartos associados a um hotel.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Array de quartos do hotel.</returns>
    Task<Room[]> GetRoomsByHotelIdAsync(long hotelId);
}
