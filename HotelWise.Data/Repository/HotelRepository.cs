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

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(ulong id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ulong id)
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
