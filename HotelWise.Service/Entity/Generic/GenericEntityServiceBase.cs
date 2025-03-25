using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using System.Linq.Expressions;

namespace HotelWise.Service.Generic
{
    public abstract class GenericEntityServiceBase<T, TDto> : IGenericService<TDto>
        where T : class, new()
        where TDto : class, new()
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly Serilog.ILogger _logger;
        protected readonly IValidator<T> _entityValidator;
        protected long UserId { get; private set; }

        // Constantes para mensagens de erro
        private const string ErrorFetchingAllEntities = "Error occurred while fetching all entities.";
        private const string ErrorFetchingEntityById = "Error occurred while fetching entity with ID {Id}.";
        private const string ErrorFindingEntities = "Error occurred while finding entities with specified criteria.";
        private const string ErrorAddingEntity = "Error occurred while adding a new entity.";
        private const string ErrorAddingEntitiesRange = "Error occurred while adding a range of new entities.";
        private const string ErrorUpdatingEntity = "Error occurred while updating the entity.";
        private const string ErrorUpdatingEntitiesRange = "Error occurred while updating a range of entities.";
        private const string ErrorDeletingEntity = "Error occurred while deleting entity with ID {Id}.";
        private const string ErrorCountingEntities = "Error occurred while counting entities.";
        private const string ErrorFetchingEntitiesPagination = "Error occurred while fetching entities with offset {Offset} and limit {Limit}.";

        private const string GeneralErrorOccurred = "An error occurred while processing the request.";

        protected GenericEntityServiceBase(IGenericRepository<T> repository, IMapper mapper, Serilog.ILogger logger, IValidator<T> entityValidator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entityValidator = entityValidator;
        }

        public void SetUserId(long id)
        {
            UserId = id;
        }

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

        public virtual async Task<ServiceResponse<TDto>> AddAsync(TDto entityDto)
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

        protected void LogAndThrow(Exception ex, string message)
        {
            _logger.Error(ex, message);
            throw new InvalidOperationException(GeneralErrorOccurred, ex);
        }
        public virtual async Task<ServiceResponse<TDto>> Validate(T item)
        {
            ServiceResponse<TDto> response = new ServiceResponse<TDto>();
            try
            {

                var validationResult = await _entityValidator.ValidateAsync(item);

                response.Success = validationResult.IsValid;
                response.Errors = HelperValidation.GetErrorsMap(validationResult).ToList();
                //Translate Message  
                if (response.Errors != null && response.Errors.Count > 0)
                {
                    List<ErrorResponse> errosTranslated = new List<ErrorResponse>();
                    foreach (var errosItem in response.Errors)
                    {
                        var errosAdd = new ErrorResponse()
                        {
                            Name = errosItem.Name,
                            Message = errosItem.DefaultMessage,

                            ErrorCode = errosItem.ErrorCode,
                        };

                        errosTranslated.Add(errosAdd);
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
}
