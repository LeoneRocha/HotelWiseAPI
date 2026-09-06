#if NET8_0_OR_GREATER
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Abstractions;
using SmartCoreHub.Core.SDK.Domain.DTOs.Common;
using SmartCoreHub.Core.SDK.Domain.Entities.Common;
using SmartCoreHub.Core.SDK.Service.Validation;
using ValidatorConstants = SmartCoreHub.Core.SDK.Service.Validation.ValidatorConstants;
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Services;

/// <summary>
/// Casca HW DTO-first local (sem SCH Obsolete Abstractions.IGenericService).
/// Preferir <c>HotelWise.Service.Generic.DtoEntityServiceBase</c> nos hosts.
/// </summary>
[Obsolete("Prefer HotelWise.Service.Generic.DtoEntityServiceBase<T,TDto> (hosts). Casca mantida para testes do SDK.")]
[SdkWrappedSource(targetType: "HotelWise.Core.SDK.Abstractions.IGenericService`1", targetPackage: "HotelWise.Core.SDK", description: "Casca DTO-first local independente de SCH Obsolete.")]
public abstract class GenericEntityServiceBase<T, TDto> : IGenericService<TDto>
    where T : LongEntityBase, new()
    where TDto : class, new()
{
    protected readonly IGenericRepository<T> _repository;
    protected readonly IMapper _mapper;
    protected readonly Serilog.ILogger _logger;
    protected readonly IValidator<T> _entityValidator;
    protected long UserId { get; private set; }

    protected GenericEntityServiceBase(
        IGenericRepository<T> repository,
        IMapper mapper,
        Serilog.ILogger logger,
        IValidator<T> entityValidator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _entityValidator = entityValidator;
    }

    public void SetUserId(long id) => UserId = id;

    public virtual async Task<List<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<TDto?> GetByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<TDto>(entity) ?? new TDto();
    }

    public virtual async Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate)
    {
        var entityPredicate = _mapper.Map<Expression<Func<T, bool>>>(predicate);
        var entities = await _repository.FindAsync(entityPredicate);
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto)
    {
        var entityAdd = _mapper.Map<T>(entityDto);
        var response = await Validate(entityAdd);
        if (response.Success)
        {
            var addedEntity = await _repository.AddAsync(entityAdd);
            response.Data = _mapper.Map<TDto>(addedEntity) ?? new TDto();
        }
        return response;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TDto> entitiesDto)
    {
        var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
        await _repository.AddRangeAsync(entities);
    }

    public virtual async Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto)
    {
        var entityAdd = _mapper.Map<T>(entityDto);
        var response = await Validate(entityAdd);
        if (response.Success)
        {
            var updatedEntity = await _repository.UpdateAsync(entityAdd);
            response.Data = _mapper.Map<TDto>(updatedEntity);
        }
        return response;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto)
    {
        var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
        await _repository.UpdateRangeAsync(entities);
    }

    public virtual async Task DeleteAsync(long id) => _ = await _repository.DeleteAsync(id);

    public virtual async Task<int> CountAsync() => await _repository.CountAsync();

    public virtual async Task<List<TDto>> FetchAsync(int offset, int limit)
    {
        var entities = await _repository.FetchAsync(offset, limit);
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<ServiceResponse<TDto>> Validate(T item)
    {
        var response = new ServiceResponse<TDto>();
        var validationResult = await _entityValidator.ValidateAsync(item);
        response.Errors = HelperValidation.GetErrorsMap(validationResult).ToList();
        if (response.Errors is { Count: > 0 })
        {
            response.Errors = response.Errors.Select(e => new ErrorResponse
            {
                Name = e.Name,
                Message = e.DefaultMessage ?? string.Empty,
                ErrorCode = e.ErrorCode,
            }).ToList();
            response.Message = ValidatorConstants.ValidateErroMessage_Message;
        }
        else
        {
            response.Message = ValidatorConstants.ValidateSuccessMessage_Message;
        }
        return response;
    }
}
#endif
