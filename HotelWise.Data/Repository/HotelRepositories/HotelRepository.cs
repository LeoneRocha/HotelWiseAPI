using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository.HotelRepositories;

/// <summary>
/// Implementação concreta do repositório de hotéis <see cref="Hotel"/> com paginação e extração de tags no MySQL.
/// </summary>
public class HotelRepository : GenericRepositoryBase<Hotel, HotelWiseDbContextMysql>, IHotelRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public HotelRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) : base(context, options)
    {
    }

    /// <summary>
    /// Obtém a quantidade total de estabelecimentos hoteleiros cadastrados.
    /// </summary>
    /// <returns>Contagem total de hotéis.</returns>
    public async Task<int> GetTotalHotelsCountAsync()
    {
        return await _dataset.AsNoTracking().CountAsync();
    }

    /// <summary>
    /// Recupera uma página de hotéis com paginação via Skip e Take.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar.</param>
    /// <param name="limit">Quantidade máxima de registros a retornar.</param>
    /// <returns>Array de hotéis da página solicitada.</returns>
    public async Task<Hotel[]> FetchHotelsAsync(int offset, int limit)
    {
        using (var context = CreateContext())
        {
            var resultRange = await context.Hotels.AsNoTracking().Skip(offset).Take(limit).ToArrayAsync();

            return resultRange;
        }
    }

    /// <summary>
    /// Obtém os arrays de tags de todos os hotéis paginados para indexação ou filtros.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar.</param>
    /// <param name="limit">Quantidade máxima de registros a processar.</param>
    /// <returns>Matriz contendo as tags associadas a cada hotel.</returns>
    public async Task<string[][]> GetAllTagsAsync(int offset, int limit)
    {
        using (var context = CreateContext())
        {
            var resultRange = await context.Hotels.AsNoTracking().Select(h => h.Tags).Skip(offset).Take(limit).ToArrayAsync();

            return resultRange;
        }
    }
}
