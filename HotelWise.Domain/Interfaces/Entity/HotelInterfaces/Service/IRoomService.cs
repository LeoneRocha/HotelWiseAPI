using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service
{
    public interface IRoomService : IGenericService<RoomDto>
    {
        Task<ServiceResponse<RoomDto>> CreateRoomAsync(RoomDto roomDto); // Criação de um novo quarto
        Task<ServiceResponse<RoomDto>> UpdateRoomAsync(long roomId, RoomDto roomDto); // Atualização de um quarto existente
        Task<ServiceResponse<string>> DeleteRoomAsync(long roomId); // Exclusão de um quarto
        Task<ServiceResponse<RoomDto[]>> GetRoomsByHotelIdAsync(long hotelId); // Recupera todos os quartos de um hotel
    }
}
