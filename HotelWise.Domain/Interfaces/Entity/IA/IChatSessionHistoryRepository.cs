using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Interfaces.Entity.IA;

/// <summary>
/// Contrato de repositório para persistência e recuperação do histórico de sessões de chat com inteligência artificial.
/// </summary>
public interface IChatSessionHistoryRepository : IGenericRepository<ChatSessionHistory>
{
    /// <summary>
    /// Exclui o histórico de chat associado ao token/GUID de sessão informado.
    /// </summary>
    /// <param name="token">Identificador único (GUID) da sessão.</param>
    /// <returns>Tarefa representando a operação assíncrona.</returns>
    Task DeleteByIdTokenAsync(string token);

    /// <summary>
    /// Obtém o histórico da sessão de chat a partir do seu token identificador (GUID).
    /// </summary>
    /// <param name="token">Identificador único (GUID) da sessão.</param>
    /// <returns>Registro de histórico da sessão ou <c>null</c> se não encontrado.</returns>
    Task<ChatSessionHistory?> GetByIdTokenAsync(string token);
}

