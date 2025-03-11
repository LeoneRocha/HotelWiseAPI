namespace HotelWise.Domain.Interfaces.Entity
{
    using System.Linq.Expressions;

    namespace HotelWise.Domain.Interfaces.Entity
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<List<T>> GetAllAsync();
            Task<T?> GetByIdAsync(long id);
            Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
            Task<T> AddAsync(T entity);
            Task AddRangeAsync(IEnumerable<T> entities);
            Task<T> UpdateAsync(T entity);
            Task UpdateRangeAsync(IEnumerable<T> entities);
            Task DeleteAsync(long id);
            Task<int> CountAsync();
            Task<List<T>> FetchAsync(int offset, int limit);
        }
    }

}