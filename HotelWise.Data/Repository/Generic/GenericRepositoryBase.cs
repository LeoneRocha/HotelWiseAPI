using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelWise.Data.Repository.Generic
{
    public abstract class GenericRepositoryBase<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dataset;
         
        private readonly DbContextOptions<DbContext>? _options;

        public GenericRepositoryBase(DbContext context, DbContextOptions<DbContext> options)
        {
            _context = context;
            _dataset = _context.Set<T>();
            _options = options;
        }

        protected DbContext CreateContext()
        {
            if (_options == null)
            {
                throw new InvalidOperationException("DbContextOptions não foi fornecido.");
            }
            return (DbContext)Activator.CreateInstance(typeof(DbContext), _options)!;
        }

        protected GenericRepositoryBase(DbContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dataset.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dataset.FindAsync(id);
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataset.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dataset.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dataset.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dataset.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dataset.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await _dataset.FindAsync(id);
            if (entity != null)
            {
                _dataset.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dataset.CountAsync();
        }

        public virtual async Task<List<T>> FetchAsync(int offset, int limit)
        {
            return await _dataset.AsNoTracking().Skip(offset).Take(limit).ToListAsync();
        }
    }
}
