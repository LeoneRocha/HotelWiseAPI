#if NET8_0_OR_GREATER
using AutoMapper;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Base mínima para serviços de vector store tipados por entidade.
/// Expõe mapper, logger e escopo de usuário (<see cref="UserId"/>) compartilhados
/// pelas implementações concretas de <see cref="Abstractions.IVectorStoreService{TEntity}"/>.
/// </summary>
/// <example>
/// <code>
/// public class HotelVectorStoreService : GenericVectorStoreServiceBase, IVectorStoreService&lt;Hotel&gt;
/// {
///     public HotelVectorStoreService(IMapper mapper, Serilog.ILogger logger)
///         : base(mapper, logger) { }
/// }
///
/// service.SetUserId(userId);
/// </code>
/// </example>
public abstract class GenericVectorStoreServiceBase
{
    /// <summary>
    /// Mapper AutoMapper para conversão entidade ↔ vetor.
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Logger estruturado.
    /// </summary>
    protected readonly Serilog.ILogger _logger;

    /// <summary>
    /// Identificador do usuário autenticado no contexto do serviço.
    /// </summary>
    protected long UserId { get; private set; }

    /// <summary>
    /// Inicializa a base com mapper e logger obrigatórios.
    /// </summary>
    /// <param name="mapper">Instância do AutoMapper.</param>
    /// <param name="logger">Logger Serilog.</param>
    protected GenericVectorStoreServiceBase(IMapper mapper, Serilog.ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Define o identificador do usuário no contexto do serviço.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    /// <example>
    /// <code>
    /// service.SetUserId(42);
    /// </code>
    /// </example>
    public void SetUserId(long id) => UserId = id;
}
#endif
