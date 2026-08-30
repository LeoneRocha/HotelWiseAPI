using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de histórico de sessões de chat <see cref="ChatSessionHistory"/> utilizando EF Core e MySQL.
/// </summary>
public class ChatSessionHistoryRepository : GenericRepositoryBase<ChatSessionHistory, HotelWiseDbContextMysql>, IChatSessionHistoryRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="ChatSessionHistoryRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto do EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public ChatSessionHistoryRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) : base(context, options)
    {
    }

    /// <summary>
    /// Exclui o histórico de mensagens vinculado ao token identificador da sessão.
    /// </summary>
    /// <param name="token">GUID da sessão a ser excluída.</param>
    /// <returns>Tarefa representando a operação assíncrona.</returns>
    public Task DeleteByIdTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Recupera o histórico da sessão de chat a partir do seu identificador GUID de sessão.
    /// </summary>
    /// <param name="token">GUID identificador da sessão de chat.</param>
    /// <returns>Entidade <see cref="ChatSessionHistory"/> correspondente ou <c>null</c> se inexistente.</returns>
    public async Task<ChatSessionHistory?> GetByIdTokenAsync(string token)
    {
        return await _dataset.AsNoTracking().FirstOrDefaultAsync(et => et.IdToken.Equals(token));
    }
}