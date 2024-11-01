using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelWiseDbContextMysql _context;

        public HotelRepository(HotelWiseDbContextMysql context)
        {
            _context = context;
        }

        public async Task<Hotel[]> GetAllAsync()
        {
            return await _context.Hotels.AsNoTracking().ToArrayAsync();
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

        public async Task DeleteAsync(long id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }
    }

}
