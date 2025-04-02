using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service
{
    public interface IRoomAvailabilityService : IGenericService<RoomAvailabilityDto>
    {
        Task<ServiceResponse<RoomAvailabilityDto>> CreateRoomAvailabilityAsync(RoomAvailabilityDto availabilityDto); // Criação de nova disponibilidade
        Task<ServiceResponse<RoomAvailabilityDto>> UpdateRoomAvailabilityAsync(long availabilityId, RoomAvailabilityDto availabilityDto); // Atualização de uma disponibilidade existente
        Task<ServiceResponse<string>> DeleteRoomAvailabilityAsync(long availabilityId); // Exclusão de uma disponibilidade
        Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesByRoomIdAsync(long roomId); // Recupera disponibilidades associadas a um quarto
    }
}
