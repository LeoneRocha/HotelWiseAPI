using AutoMapper;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using System.Linq.Expressions;

namespace HotelWise.Service.Generic
{
    public abstract class GenericServiceBase<T, TDto> : IGenericService<TDto>
        where T : class, new()
        where TDto : class, new()
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly Serilog.ILogger _logger;

        protected long UserId { get; private set; }

        // Constantes para mensagens
        private const string FetchingAllEntitiesMessage = "Fetching all entities.";
        private const string FetchEntityByIdMessage = "Fetching entity with ID {0}.";
        private const string FindingEntitiesMessage = "Finding entities with specified criteria.";
        private const string AddingEntityMessage = "Adding new entity.";
        private const string AddingEntitiesRangeMessage = "Adding a range of new entities.";
        private const string UpdatingEntityMessage = "Updating entity.";
        private const string UpdatingEntitiesRangeMessage = "Updating a range of entities.";
        private const string DeletingEntityMessage = "Deleting entity with ID {0}.";
        private const string CountingEntitiesMessage = "Counting all entities.";
        private const string FetchingEntitiesPaginationMessage = "Fetching entities with offset {0} and limit {1}.";

        private const string ErrorOccurredMessage = "An error occurred while processing the request.";

        protected GenericServiceBase(IGenericRepository<T> repository, IMapper mapper, Serilog.ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetUserId(long id)
        {
            UserId = id;
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            try
            {
                _logger.Information(FetchingAllEntitiesMessage);
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<List<TDto>>(entities);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, FetchingAllEntitiesMessage);
                return new List<TDto>();
            }
        }

        public virtual async Task<TDto?> GetByIdAsync(long id)
        {
            try
            {
                _logger.Information(string.Format(FetchEntityByIdMessage, id));
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<TDto>(entity) ?? new TDto();
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, string.Format(FetchEntityByIdMessage, id));
                return null;
            }
        }

        public virtual async Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate)
        {
            try
            {
                _logger.Information(FindingEntitiesMessage);
                var entityPredicate = _mapper.Map<Expression<Func<T, bool>>>(predicate);
                var entities = await _repository.FindAsync(entityPredicate);
                return _mapper.Map<List<TDto>>(entities);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, FindingEntitiesMessage);
                return new List<TDto>();
            }
        }

        public virtual async Task<TDto> AddAsync(TDto entityDto)
        {
            try
            {
                _logger.Information(AddingEntityMessage);
                var entity = _mapper.Map<T>(entityDto);
                var addedEntity = await _repository.AddAsync(entity);
                return _mapper.Map<TDto>(addedEntity) ?? new TDto();
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, AddingEntityMessage);
                return new TDto();
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<TDto> entitiesDto)
        {
            try
            {
                _logger.Information(AddingEntitiesRangeMessage);
                var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
                await _repository.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, AddingEntitiesRangeMessage);
            }
        }

        public virtual async Task<TDto> UpdateAsync(TDto entityDto)
        {
            try
            {
                _logger.Information(UpdatingEntityMessage);
                var entity = _mapper.Map<T>(entityDto);
                var updatedEntity = await _repository.UpdateAsync(entity);
                return _mapper.Map<TDto>(updatedEntity) ?? new TDto();
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, UpdatingEntityMessage);
                return new TDto();
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto)
        {
            try
            {
                _logger.Information(UpdatingEntitiesRangeMessage);
                var entities = _mapper.Map<IEnumerable<T>>(entitiesDto);
                await _repository.UpdateRangeAsync(entities);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, UpdatingEntitiesRangeMessage);
            }
        }

        public virtual async Task DeleteAsync(long id)
        {
            try
            {
                _logger.Information(string.Format(DeletingEntityMessage, id));
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, string.Format(DeletingEntityMessage, id));
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                _logger.Information(CountingEntitiesMessage);
                return await _repository.CountAsync();
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, CountingEntitiesMessage);
                return 0;
            }
        }

        public virtual async Task<List<TDto>> FetchAsync(int offset, int limit)
        {
            try
            {
                _logger.Information(string.Format(FetchingEntitiesPaginationMessage, offset, limit));
                var entities = await _repository.FetchAsync(offset, limit);
                return _mapper.Map<List<TDto>>(entities);
            }
            catch (Exception ex)
            {
                LogAndThrow(ex, string.Format(FetchingEntitiesPaginationMessage, offset, limit));
                return new List<TDto>();
            }
        }

        protected void LogAndThrow(Exception ex, string message)
        {
            _logger.Error(ex, message);
            throw new InvalidOperationException(ErrorOccurredMessage, ex);
        }
    }
}
