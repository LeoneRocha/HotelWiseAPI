using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room?> FindByRoomIdAsNoTracking(long roomId);
        Task<Room[]> GetRoomsByHotelAsNoTracking(long hotelId);
        Task<Room[]> GetRoomsByHotelIdAsync(long hotelId);
    }
}
