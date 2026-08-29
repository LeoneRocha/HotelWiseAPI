using System.Linq.Expressions;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de serviço de aplicação orientado a DTOs.
/// Expõe operações de consulta, criação, atualização e exclusão sobre <typeparamref name="TDto"/>,
/// retornando respostas padronizadas quando aplicável e permitindo contextualizar o usuário autenticado.
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de entidade manipulado pelo serviço.</typeparam>
public interface IGenericService<TDto> where TDto : class
{
    /// <summary>
    /// Define o identificador do usuário autenticado no contexto das operações do serviço
    /// (auditoria, permissões e rastreio).
    /// </summary>
    /// <param name="id">Identificador do usuário a associar ao contexto do serviço.</param>
    void SetUserId(long id);

    /// <summary>
    /// Obtém todos os registros do tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <returns>Lista de DTOs encontrados; lista vazia se não houver registros.</returns>
    Task<List<TDto>> GetAllAsync();

    /// <summary>
    /// Obtém um registro pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do registro.</param>
    /// <returns>DTO correspondente ou <c>null</c> se não encontrado.</returns>
    Task<TDto?> GetByIdAsync(long id);

    /// <summary>
    /// Busca registros que satisfaçam o predicado informado.
    /// </summary>
    /// <param name="predicate">Expressão de filtro sobre o DTO.</param>
    /// <returns>Lista de DTOs que atendem ao predicado.</returns>
    Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);

    /// <summary>
    /// Cria um novo registro a partir do DTO informado.
    /// </summary>
    /// <param name="entityDto">DTO com os dados a persistir.</param>
    /// <returns>Resposta padronizada contendo o DTO criado (ou erros de validação/negócio).</returns>
    Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto);

    /// <summary>
    /// Insere em lote uma coleção de DTOs.
    /// </summary>
    /// <param name="entitiesDto">Coleção de DTOs a inserir.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task AddRangeAsync(IEnumerable<TDto> entitiesDto);

    /// <summary>
    /// Atualiza um registro existente a partir do DTO informado.
    /// </summary>
    /// <param name="entityDto">DTO com os dados atualizados (incluindo identificador).</param>
    /// <returns>Resposta padronizada contendo o DTO atualizado (ou erros de validação/negócio).</returns>
    Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto);

    /// <summary>
    /// Atualiza em lote uma coleção de DTOs.
    /// </summary>
    /// <param name="entitiesDto">Coleção de DTOs a atualizar.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto);

    /// <summary>
    /// Exclui o registro identificado por <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Identificador do registro a excluir.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// Conta a quantidade total de registros do tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <returns>Número total de registros.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Obtém uma página de registros com deslocamento e limite.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar (deslocamento).</param>
    /// <param name="limit">Quantidade máxima de registros a retornar.</param>
    /// <returns>Lista paginada de DTOs.</returns>
    Task<List<TDto>> FetchAsync(int offset, int limit);
}
