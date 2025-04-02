using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository.HotelRepositories
{
    public class RoomAvailabilityRepository : GenericRepositoryBase<RoomAvailability, HotelWiseDbContextMysql>, IRoomAvailabilityRepository
    {
        public RoomAvailabilityRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
            : base(context, options) { }

        /// <summary>
        /// Retorna as disponibilidades de quartos para o RoomId especificado como array.
        /// </summary>
        public async Task<RoomAvailability[]> GetAvailabilityByRoomId(long roomId)
        {
            return await _dataset
                .AsNoTracking()
                .Where(ra => ra.RoomId == roomId)
                .ToArrayAsync(); // Agora retorna como array
        }

        /// <summary>
        /// Retorna as disponibilidades de quartos dentro do intervalo de datas como array.
        /// </summary>
        public async Task<RoomAvailability[]> GetAvailabilityByDateRange(long roomId, DateTime startDate, DateTime endDate)
        {
            return await _dataset
                .AsNoTracking()
                .Where(ra => ra.RoomId == roomId &&
                             ra.StartDate <= endDate &&
                             ra.EndDate >= startDate)
                .ToArrayAsync(); // Agora retorna como array
        }
    }
}
