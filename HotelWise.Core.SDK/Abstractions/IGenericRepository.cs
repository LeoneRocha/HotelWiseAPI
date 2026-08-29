using System.Linq.Expressions;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de repositório para persistência de entidades de domínio.
/// Define operações CRUD, consulta por predicado, contagem, paginação e verificação de existência
/// sobre o tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada pelo repositório.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Obtém todas as entidades do tipo <typeparamref name="T"/>.
    /// </summary>
    /// <returns>Lista de entidades; lista vazia se não houver registros.</returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// Obtém uma entidade pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>Entidade correspondente ou <c>null</c> se não encontrada.</returns>
    Task<T?> GetByIdAsync(long id);

    /// <summary>
    /// Busca entidades que satisfaçam o predicado informado.
    /// </summary>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <returns>Lista de entidades que atendem ao predicado.</returns>
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Insere uma nova entidade.
    /// </summary>
    /// <param name="entity">Entidade a persistir.</param>
    /// <returns>Entidade inserida (possivelmente com identificador gerado).</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Insere em lote uma coleção de entidades.
    /// </summary>
    /// <param name="entities">Coleção de entidades a inserir.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    /// <param name="entity">Entidade com os dados atualizados.</param>
    /// <returns>Entidade atualizada.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Atualiza em lote uma coleção de entidades.
    /// </summary>
    /// <param name="entities">Coleção de entidades a atualizar.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task UpdateRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Exclui a entidade identificada por <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Identificador da entidade a excluir.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// Conta a quantidade total de entidades do tipo <typeparamref name="T"/>.
    /// </summary>
    /// <returns>Número total de entidades.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Obtém uma página de entidades com deslocamento e limite.
    /// </summary>
    /// <param name="offset">Quantidade de entidades a ignorar (deslocamento).</param>
    /// <param name="limit">Quantidade máxima de entidades a retornar.</param>
    /// <returns>Lista paginada de entidades.</returns>
    Task<List<T>> FetchAsync(int offset, int limit);

    /// <summary>
    /// Verifica se existe ao menos uma entidade que satisfaça o predicado.
    /// </summary>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <returns><c>true</c> se existir ao menos uma correspondência; caso contrário, <c>false</c>.</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
