using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class RoomRepository : GenericRepositoryBase<Room, HotelWiseDbContextMysql>, IRoomRepository
    {
        public RoomRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
            : base(context, options) { }

        public async Task<Room?> FindByIdWithHotel(long roomId)
        {
            return await _dataset
                .Include(r => r.Hotel)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }
         
        public async Task<Room[]> GetRoomsByHotel(long hotelId)
        {
            return await _dataset
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .ToArrayAsync();
        }
    }
}
