using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository.HotelRepositories;

/// <summary>
/// Implementação concreta do repositório de hotéis <see cref="Hotel"/> com paginação e extração de tags no MySQL.
/// </summary>
public class HotelRepository : GenericRepository<Hotel, HotelWiseDbContextMysql>, IHotelRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public HotelRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options)
    {
    }

    /// <inheritdoc />
    public async Task<int> GetTotalHotelsCountAsync()
    {
        return await _dbSet.AsNoTracking().CountAsync();
    }

    /// <inheritdoc />
    public async Task<Hotel[]> FetchHotelsAsync(int offset, int limit)
    {
        using (var context = CreateContext())
        {
            var resultRange = await context.Hotels.AsNoTracking().Skip(offset).Take(limit).ToArrayAsync();

            return resultRange;
        }
    }

    /// <inheritdoc />
    public async Task<string[][]> GetAllTagsAsync(int offset, int limit)
    {
        using (var context = CreateContext())
        {
            var resultRange = await context.Hotels.AsNoTracking().Select(h => h.Tags).Skip(offset).Take(limit).ToArrayAsync();

            return resultRange;
        }
    }
}
