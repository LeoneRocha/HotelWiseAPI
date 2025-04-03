using HotelWise.Domain.Dto;
using System.Linq.Expressions;

namespace HotelWise.Domain.Interfaces.Base
{
    public interface IGenericService<TDto> where TDto : class
    {
        void SetUserId(long id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(long id);
        Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);
        Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto);
        Task AddRangeAsync(IEnumerable<TDto> entitiesDto);
        Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto);
        Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto);
        Task DeleteAsync(long id);
        Task<int> CountAsync();
        Task<List<TDto>> FetchAsync(int offset, int limit);
    }
}
