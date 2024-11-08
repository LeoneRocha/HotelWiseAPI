using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelWiseDbContextMysql _context; 
        private readonly DbContextOptions<HotelWiseDbContextMysql> _options;

        public HotelRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        {
            _context = context;
            _options = options;
        }

        private HotelWiseDbContextMysql CreateContext()
        {
            return new HotelWiseDbContextMysql(_options);
        }

        public async Task<Hotel[]> GetAll()
        {
            return await _context.Hotels.AsNoTracking().ToArrayAsync();
        }
        public async Task<Hotel[]> GetAllAsync()
        {
            return await _context.Hotels.ToArrayAsync();
        }

        public async Task<Hotel?> GetByIdAsync(long id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(Hotel[] hotels)
        {
            await _context.Hotels.AddRangeAsync(hotels);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRangeAsync(Hotel[] hotel)
        {
            _context.Hotels.UpdateRange(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(long id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalHotelsCountAsync()
        {
            return await _context.Hotels.AsNoTracking().CountAsync();
        }
        public async Task<Hotel[]> FetchHotelsAsync(int offset, int limit)
        {
            using (var context = CreateContext())
            {
                var resultRange = await context.Hotels.AsNoTracking().Skip(offset).Take(limit).ToArrayAsync();

                return resultRange;
            }
        } 
    }
}