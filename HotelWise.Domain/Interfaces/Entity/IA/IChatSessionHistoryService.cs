using HotelWise.Core.SDK.Abstractions;
using HotelWise.Domain.Dto.IA;

namespace HotelWise.Domain.Interfaces.Entity.IA;

/// <summary>
/// Contrato de serviço para manipulação, armazenamento e limpeza do histórico de conversas do assistente virtual.
/// </summary>
public interface IChatSessionHistoryService : IGenericService<ChatSessionHistoryDto>
{
    /// <summary>
    /// Recupera o histórico de mensagens de uma sessão pelo token/GUID identificador.
    /// </summary>
    /// <param name="token">Identificador único (GUID) da sessão.</param>
    /// <returns>DTO contendo o histórico da sessão ou <c>null</c> se não encontrado.</returns>
    Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string token);

    /// <summary>
    /// Remove permanentemente o histórico da sessão associada ao token/GUID informado.
    /// </summary>
    /// <param name="token">Identificador único (GUID) da sessão.</param>
    /// <returns>Tarefa representando a operação assíncrona.</returns>
    Task DeleteByIdTokenAsync(string token);
}
