using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<Reservation[]> GetByRoomId(long roomId);
        Task<Reservation[]> GetReservationsByRoomIdAsync(long roomId);
        Task<Reservation[]> GetReservationsWithinDateRange(DateTime startDate, DateTime endDate);
    }
}
