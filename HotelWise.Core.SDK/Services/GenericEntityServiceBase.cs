#if NET8_0_OR_GREATER
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Constants;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Validation;

namespace HotelWise.Core.SDK.Services;

/// <summary>
/// Serviço genérico de entidade com CRUD assíncrono, validação FluentValidation
/// e mapeamento AutoMapper entre entidade <typeparamref name="T"/> e DTO
/// <typeparamref name="TDto"/>. Serve de base para serviços de domínio concretos
/// do HotelWise, encapsulando logging de erros e respostas <see cref="ServiceResponse{TDto}"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
/// <typeparam name="TDto">Tipo do DTO exposto pela camada de serviço.</typeparam>
/// <example>
/// <code>
/// public class HotelService : GenericEntityServiceBase&lt;Hotel, HotelDto&gt;
/// {
///     public HotelService(IGenericRepository&lt;Hotel&gt; repo, IMapper mapper,
///         Serilog.ILogger logger, IValidator&lt;Hotel&gt; validator)
///         : base(repo, mapper, logger, validator) { }
/// }
/// </code>
/// </example>
public abstract class GenericEntityServiceBase<T, TDto> : IGenericService<TDto>
    where T : class, new()
    where TDto : class, new()
{
    /// <summary>
    /// Repositório genérico da entidade <typeparamref name="T"/>.
    /// </summary>
    protected readonly IGenericRepository<T> _repository;

    /// <summary>
    /// Mapper AutoMapper entre entidade e DTO.
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Logger Serilog para erros de operação.
    /// </summary>
    protected readonly Serilog.ILogger _logger;

    /// <summary>
    /// Validador FluentValidation da entidade.
    /// </summary>
    protected readonly IValidator<T> _entityValidator;

    /// <summary>
    /// Identificador do usuário autenticado associado ao contexto do serviço.
    /// </summary>
    protected long UserId { get; private set; }

    /// <summary>Mensagem de erro ao buscar todas as entidades.</summary>
    private const string ErrorFetchingAllEntities = "Error occurred while fetching all entities.";
    /// <summary>Mensagem de erro ao buscar entidade por Id.</summary>
    private const string ErrorFetchingEntityById = "Error occurred while fetching entity with ID {Id}.";
    /// <summary>Mensagem de erro ao filtrar entidades.</summary>
    private const string ErrorFindingEntities = "Error occurred while finding entities with specified criteria.";
    /// <summary>Mensagem de erro ao adicionar entidade.</summary>
    private const string ErrorAddingEntity = "Error occurred while adding a new entity.";
    /// <summary>Mensagem de erro ao adicionar intervalo de entidades.</summary>
    private const string ErrorAddingEntitiesRange = "Error occurred while adding a range of new entities.";
    /// <summary>Mensagem de erro ao atualizar entidade.</summary>
    private const string ErrorUpdatingEntity = "Error occurred while updating the entity.";
    /// <summary>Mensagem de erro ao atualizar intervalo de entidades.</summary>
    private const string ErrorUpdatingEntitiesRange = "Error occurred while updating a range of entities.";
    /// <summary>Mensagem de erro ao excluir entidade.</summary>
    private const string ErrorDeletingEntity = "Error occurred while deleting entity with ID {Id}.";
    /// <summary>Mensagem de erro ao contar entidades.</summary>
    private const string ErrorCountingEntities = "Error occurred while counting entities.";
    /// <summary>Mensagem de erro na paginação.</summary>
    private const string ErrorFetchingEntitiesPagination = "Error occurred while fetching entities with offset {Offset} and limit {Limit}.";
    /// <summary>Mensagem genérica encapsulada na exceção relançada.</summary>
    private const string GeneralErrorOccurred = "An error occurred while processing the request.";

    /// <summary>
    /// Inicializa o serviço genérico com repositório, mapper, logger e validador.
    /// </summary>
    /// <param name="repository">Repositório da entidade.</param>
    /// <param name="mapper">Instância AutoMapper.</param>
    /// <param name="logger">Logger Serilog.</param>
    /// <param name="entityValidator">Validador FluentValidation da entidade.</param>
    /// <exception cref="ArgumentNullException">Quando repository, mapper ou logger são nulos.</exception>
    protected GenericEntityServiceBase(IGenericRepository<T> repository, IMapper mapper, Serilog.ILogger logger, IValidator<T> entityValidator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _entityValidator = entityValidator;
    }

    /// <summary>
    /// Define o Id do usuário no contexto deste serviço.
    /// </summary>
    /// <param name="id">Identificador do usuário autenticado.</param>
    public void SetUserId(long id) => UserId = id;

    /// <summary>
    /// Obtém todos os registros mapeados para <typeparamref name="TDto"/>.
    /// </summary>
    /// <returns>Lista de DTOs.</returns>
    public virtual async Task<List<TDto>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TDto>>(entities);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorFetchingAllEntities);
            return new List<TDto>();
        }
    }

    /// <summary>
    /// Obtém um registro pelo identificador, mapeado para <typeparamref name="TDto"/>.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>DTO correspondente ou valor padrão em falha tratada.</returns>
    public virtual async Task<TDto?> GetByIdAsync(long id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity) ?? new TDto();
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorFetchingEntityById.Replace("{Id}", id.ToString()));
            return null;
        }
    }

    /// <summary>
    /// Filtra registros com predicado sobre o DTO (mapeado para predicado de entidade).
    /// </summary>
    /// <param name="predicate">Expressão de filtro sobre <typeparamref name="TDto"/>.</param>
    /// <returns>Lista de DTOs que satisfazem o critério.</returns>
    public virtual async Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate)
    {
        try
        {
            var entityPredicate = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var entities = await _repository.FindAsync(entityPredicate);
            return _mapper.Map<List<TDto>>(entities);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorFindingEntities);
            return new List<TDto>();
        }
    }

    /// <summary>
    /// Valida e cria uma nova entidade a partir do DTO.
    /// </summary>
    /// <param name="entityDto">DTO a persistir.</param>
    /// <returns><see cref="ServiceResponse{TDto}"/> com dados ou erros de validação.</returns>
    public virtual async Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto)
    {
        ServiceResponse<TDto> response = new ServiceResponse<TDto>();
        try
        {
            var entityAdd = _mapper.Map<T>(entityDto);
            response = await Validate(entityAdd);
            if (response.Success)
            {
                var addedEntity = await _repository.AddAsync(entityAdd);
                response.Data = _mapper.Map<TDto>(addedEntity) ?? new TDto();
            }
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorAddingEntity);
        }
        return response;
    }

    /// <summary>
    /// Adiciona um conjunto de entidades a partir de DTOs (sem validação individual).
    /// </summary>
    /// <param name="entitiesDto">Coleção de DTOs a inserir.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task AddRangeAsync(IEnumerable<TDto> entitiesDto)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
            await _repository.AddRangeAsync(entities);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorAddingEntitiesRange);
        }
    }

    /// <summary>
    /// Valida e atualiza uma entidade a partir do DTO.
    /// </summary>
    /// <param name="entityDto">DTO com os novos valores.</param>
    /// <returns><see cref="ServiceResponse{TDto}"/> com dados ou erros de validação.</returns>
    public virtual async Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto)
    {
        ServiceResponse<TDto> response = new ServiceResponse<TDto>();
        try
        {
            var entityAdd = _mapper.Map<T>(entityDto);
            response = await Validate(entityAdd);
            if (response.Success)
            {
                var updatedEntity = await _repository.UpdateAsync(entityAdd);
                response.Data = _mapper.Map<TDto>(updatedEntity);
            }
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorUpdatingEntity);
        }
        return response;
    }

    /// <summary>
    /// Atualiza um conjunto de entidades a partir de DTOs.
    /// </summary>
    /// <param name="entitiesDto">Coleção de DTOs a atualizar.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
            await _repository.UpdateRangeAsync(entities);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorUpdatingEntitiesRange);
        }
    }

    /// <summary>
    /// Exclui a entidade pelo identificador.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>Tarefa que representa a operação assíncrona.</returns>
    public virtual async Task DeleteAsync(long id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorDeletingEntity.Replace("{Id}", id.ToString()));
        }
    }

    /// <summary>
    /// Conta o total de entidades no repositório.
    /// </summary>
    /// <returns>Quantidade de registros.</returns>
    public virtual async Task<int> CountAsync()
    {
        try
        {
            return await _repository.CountAsync();
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorCountingEntities);
            return 0;
        }
    }

    /// <summary>
    /// Obtém uma página de DTOs (offset/limit).
    /// </summary>
    /// <param name="offset">Quantidade de registros a ignorar.</param>
    /// <param name="limit">Quantidade máxima a retornar.</param>
    /// <returns>Lista paginada de DTOs.</returns>
    public virtual async Task<List<TDto>> FetchAsync(int offset, int limit)
    {
        try
        {
            var entities = await _repository.FetchAsync(offset, limit);
            return _mapper.Map<List<TDto>>(entities);
        }
        catch (Exception ex)
        {
            LogAndThrow(ex, ErrorFetchingEntitiesPagination.Replace("{Offset}", offset.ToString()).Replace("{Limit}", limit.ToString()));
            return new List<TDto>();
        }
    }

    /// <summary>
    /// Registra o erro no logger e relança como <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <param name="ex">Exceção original.</param>
    /// <param name="message">Mensagem de contexto para o log.</param>
    protected void LogAndThrow(Exception ex, string message)
    {
        _logger.Error(ex, message);
        throw new InvalidOperationException(GeneralErrorOccurred, ex);
    }

    /// <summary>
    /// Executa a validação FluentValidation da entidade e monta <see cref="ServiceResponse{TDto}"/>.
    /// </summary>
    /// <param name="item">Entidade a validar.</param>
    /// <returns>Resposta com Success, Errors e Message padronizados.</returns>
    public virtual async Task<ServiceResponse<TDto>> Validate(T item)
    {
        ServiceResponse<TDto> response = new ServiceResponse<TDto>();
        try
        {
            var validationResult = await _entityValidator.ValidateAsync(item);
            response.Success = validationResult.IsValid;
            response.Errors = HelperValidation.GetErrorsMap(validationResult).ToList();
            if (response.Errors != null && response.Errors.Count > 0)
            {
                List<ErrorResponse> errosTranslated = new List<ErrorResponse>();
                foreach (var errosItem in response.Errors)
                {
                    errosTranslated.Add(new ErrorResponse
                    {
                        Name = errosItem.Name,
                        Message = errosItem.DefaultMessage,
                        ErrorCode = errosItem.ErrorCode,
                    });
                }
                response.Errors = errosTranslated;
                response.Message = ValidatorConstants.ValidateErroMessage_Message;
            }
            else
            {
                response.Message = ValidatorConstants.ValidateSuccessMessage_Message;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ValidatorConstants.Generic_Erro_Message;
            _logger.Error(ex, "Validate: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
        }
        return response;
    }
}
#endif
