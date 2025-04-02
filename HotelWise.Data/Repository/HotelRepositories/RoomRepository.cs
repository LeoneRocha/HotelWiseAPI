using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class RoomRepository : GenericRepositoryBase<Room, HotelWiseDbContextMysql>, IRoomRepository
    {
        public RoomRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
            : base(context, options) { }

        public async Task<Room?> FindByRoomIdAsNoTracking(long roomId)
        {
            return await _dataset
                .Include(r => r.Hotel)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }
        /// <summary>
        /// Recupera todos os quartos associados a um hotel específico.
        /// </summary>
        public async Task<Room[]> GetRoomsByHotelIdAsync(long hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.RoomAvailabilities) // Inclui disponibilidades relacionadas
                .ToArrayAsync();
        }
        public async Task<Room[]> GetRoomsByHotelAsNoTracking(long hotelId)
        {
            return await _dataset
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .ToArrayAsync();
        }
         
    }
}
