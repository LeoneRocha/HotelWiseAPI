using HotelWise.Core.SDK.Abstractions;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;

/// <summary>
/// Contrato de repositório específico para consultas e operações de persistência da entidade <see cref="Hotel"/>.
/// </summary>
public interface IHotelRepository : IGenericRepository<Hotel>
{
    /// <summary>
    /// Obtém a contagem total de hotéis cadastrados no banco de dados.
    /// </summary>
    /// <returns>Quantidade total de hotéis.</returns>
    Task<int> GetTotalHotelsCountAsync();

    /// <summary>
    /// Recupera uma página de hotéis com paginação baseada em deslocamento e limite.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar.</param>
    /// <param name="limit">Quantidade máxima de registros a retornar.</param>
    /// <returns>Array de instâncias de <see cref="Hotel"/> encontradas.</returns>
    Task<Hotel[]> FetchHotelsAsync(int offset, int limit);

    /// <summary>
    /// Obtém todas as tags cadastradas para os hotéis com suporte a paginação.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar.</param>
    /// <param name="limit">Quantidade máxima de registros a processar.</param>
    /// <returns>Matriz contendo as coleções de tags por hotel.</returns>
    Task<string[][]> GetAllTagsAsync(int offset, int limit);
}
