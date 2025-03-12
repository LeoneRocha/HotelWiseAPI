using System.Linq.Expressions;

namespace HotelWise.Service.Generic
{
    public interface IGenericService<TDto> where TDto : class
    {
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(long id);
        Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);
        Task<TDto> AddAsync(TDto entityDto);
        Task AddRangeAsync(IEnumerable<TDto> entitiesDto);
        Task<TDto> UpdateAsync(TDto entityDto);
        Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto);
        Task DeleteAsync(long id);
        Task<int> CountAsync();
        Task<List<TDto>> FetchAsync(int offset, int limit);
    }
}
