using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service
{
    public interface IRoomService : IGenericService<RoomDto>
    { 
        Task<ServiceResponse<RoomDto[]>> GetRoomsByHotelIdAsync(long hotelId); // Recupera todos os quartos de um hotel
    }
}
