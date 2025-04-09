using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository
{
    public interface IRoomAvailabilityRepository : IGenericRepository<RoomAvailability>
    {
        /// <summary>
        /// Retorna as disponibilidades de quartos como um array.
        /// </summary>
        Task<RoomAvailability[]> GetAvailabilityByRoomId(long roomId);

        /// <summary>
        /// Retorna a disponibilidade de quartos dentro de um intervalo de datas como um array.
        /// </summary>
        Task<RoomAvailability[]> GetAvailabilityByDateRange(long roomId, DateTime startDate, DateTime endDate);
        Task<RoomAvailability[]> GetAvailabilitiesByHotelAndPeriodAsync(HotelAvailabilityRequestDto request);
    }
}
