#if NET8_0_OR_GREATER
using System.Linq.Expressions;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Repositório genérico baseado em Entity Framework Core.
/// Fornece operações CRUD assíncronas padrão sobre um <see cref="DbSet{T}"/>,
/// permitindo que repositórios concretos herdem e especializem o comportamento
/// sem reimplementar o boilerplate de persistência.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio mapeada.</typeparam>
/// <typeparam name="TContext">Tipo do <see cref="DbContext"/> utilizado.</typeparam>
public abstract class GenericRepositoryBase<T, TContext> : IGenericRepository<T>
    where T : class
    where TContext : DbContext
{
    /// <summary>
    /// Instância do contexto EF Core associada a este repositório.
    /// </summary>
    protected readonly TContext _context;

    /// <summary>
    /// Conjunto de entidades (<see cref="DbSet{T}"/>) da entidade <typeparamref name="T"/>.
    /// </summary>
    protected readonly DbSet<T> _dataset;

    /// <summary>
    /// Opções do DbContext usadas para criar novos contextos sob demanda.
    /// </summary>
    private readonly DbContextOptions<TContext>? _options;

    /// <summary>
    /// Inicializa o repositório com o contexto e as opções do EF Core.
    /// </summary>
    /// <param name="context">Contexto de banco de dados ativo.</param>
    /// <param name="options">Opções usadas para instanciar novos contextos via <see cref="CreateContext"/>.</param>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="context"/> é nulo.</exception>
    protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dataset = _context.Set<T>();
        _options = options;
    }

    /// <summary>
    /// Cria uma nova instância de <typeparamref name="TContext"/> a partir das opções armazenadas.
    /// </summary>
    /// <returns>Novo contexto EF Core tipado.</returns>
    /// <exception cref="InvalidOperationException">Quando as opções não foram fornecidas no construtor.</exception>
    protected TContext CreateContext()
    {
        if (_options == null)
        {
            throw new InvalidOperationException("DbContextOptions was not provided.");
        }
        return (TContext)Activator.CreateInstance(typeof(TContext), _options)!;
    }

    /// <summary>
    /// Obtém todas as entidades do conjunto, sem rastreamento.
    /// </summary>
    /// <returns>Lista de entidades <typeparamref name="T"/>.</returns>
    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dataset.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Busca uma entidade pela chave primária numérica.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>A entidade encontrada ou <c>null</c> se não existir.</returns>
    public virtual async Task<T?> GetByIdAsync(long id)
    {
        return await _dataset.FindAsync(id);
    }

    /// <summary>
    /// Filtra entidades conforme um predicado LINQ.
    /// </summary>
    /// <param name="predicate">Expressão de filtro aplicada ao conjunto.</param>
    /// <returns>Lista de entidades que satisfazem o predicado.</returns>
    public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dataset.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Insere uma entidade e persiste as alterações.
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada.</param>
    /// <returns>A entidade adicionada (após SaveChanges).</returns>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dataset.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Insere um conjunto de entidades e persiste as alterações.
    /// </summary>
    /// <param name="entities">Coleção de entidades a adicionar.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dataset.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza uma entidade e persiste as alterações.
    /// </summary>
    /// <param name="entity">Entidade com os novos valores.</param>
    /// <returns>A entidade atualizada.</returns>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dataset.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Atualiza um conjunto de entidades e persiste as alterações.
    /// </summary>
    /// <param name="entities">Coleção de entidades a atualizar.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dataset.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove a entidade com o identificador informado, se existir.
    /// </summary>
    /// <param name="id">Identificador da entidade a remover.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task DeleteAsync(long id)
    {
        var entity = await _dataset.FindAsync(id);
        if (entity != null)
        {
            _dataset.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Conta o total de entidades no conjunto.
    /// </summary>
    /// <returns>Quantidade de registros.</returns>
    public virtual async Task<int> CountAsync()
    {
        return await _dataset.CountAsync();
    }

    /// <summary>
    /// Verifica se existe alguma entidade que satisfaça o predicado.
    /// </summary>
    /// <param name="predicate">Expressão de filtro.</param>
    /// <returns><c>true</c> se existir ao menos um registro correspondente; caso contrário, <c>false</c>.</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dataset.AsNoTracking().AnyAsync(predicate);
    }

    /// <summary>
    /// Obtém uma página de entidades sem rastreamento.
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar (skip).</param>
    /// <param name="limit">Quantidade máxima de registros a retornar (take).</param>
    /// <returns>Lista paginada de entidades.</returns>
    public virtual async Task<List<T>> FetchAsync(int offset, int limit)
    {
        return await _dataset.AsNoTracking().Skip(offset).Take(limit).ToListAsync();
    }
}
#endif
