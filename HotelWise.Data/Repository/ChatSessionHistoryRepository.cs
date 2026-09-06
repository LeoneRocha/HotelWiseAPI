using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de histórico de sessões de chat <see cref="ChatSessionHistory"/> utilizando EF Core e MySQL.
/// </summary>
public class ChatSessionHistoryRepository : GenericRepository<ChatSessionHistory, HotelWiseDbContextMysql>, IChatSessionHistoryRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="ChatSessionHistoryRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto do EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public ChatSessionHistoryRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options)
    {
    }

    /// <inheritdoc />
    public Task DeleteByIdTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<ChatSessionHistory?> GetByIdTokenAsync(string token)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(et => et.IdToken.Equals(token));
    }
}
