using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room?> FindByIdWithHotel(long roomId);
        Task<Room[]> GetRoomsByHotel(long hotelId);
    }
}
