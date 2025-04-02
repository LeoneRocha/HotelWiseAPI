using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class ReservationRepository : GenericRepositoryBase<Reservation, HotelWiseDbContextMysql>, IReservationRepository
    {
        public ReservationRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
            : base(context, options) { }

        public async Task<Reservation[]> GetByRoomId(long roomId)
        {
            return await _dataset
                .AsNoTracking()
                .Where(r => r.RoomId == roomId)
                .ToArrayAsync();
        }

        

        /// <summary>
        /// Recupera todas as reservas associadas a um quarto específico.
        /// </summary>
        public async  Task<Reservation[]> GetReservationsByRoomIdAsync(long roomId)
        {
            return await _context.Reservations
                .Where(r => r.RoomId == roomId)
                .Include(r => r.Room) // Inclui os detalhes do quarto, se necessário
                .ToArrayAsync();
        }

        public async Task<Reservation[]> GetReservationsWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return await _dataset
                .AsNoTracking()
                .Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate)
                .ToArrayAsync();
        }
    }
}
