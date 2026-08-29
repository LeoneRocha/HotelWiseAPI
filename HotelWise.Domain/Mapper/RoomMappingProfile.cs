using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

/// <summary>
/// Perfil AutoMapper para mapeamento entre a entidade de quarto <see cref="Room"/> e seu DTO <see cref="RoomDto"/>.
/// </summary>
public class RoomMappingProfile : Profile
{
    /// <summary>
    /// Configura o mapeamento de quartos incluindo a coleção de disponibilidades associadas.
    /// </summary>
    public RoomMappingProfile()
    {
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.RoomAvailabilities));
        CreateMap<RoomDto, Room>(MemberList.None);
    }
}
