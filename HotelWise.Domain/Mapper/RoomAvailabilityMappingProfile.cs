using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

/// <summary>
/// Perfil AutoMapper para mapeamento entre a entidade <see cref="RoomAvailability"/> e o DTO <see cref="RoomAvailabilityDto"/>.
/// </summary>
public class RoomAvailabilityMappingProfile : Profile
{
    /// <summary>
    /// Configura o mapeamento de disponibilidades e lista de itens de tarifação.
    /// </summary>
    public RoomAvailabilityMappingProfile()
    {
        CreateMap<RoomAvailability, RoomAvailabilityDto>()
            .ForMember(dest => dest.AvailabilityWithPrice, opt => opt.MapFrom(src => src.AvailabilityWithPrice));
        CreateMap<RoomAvailabilityDto, RoomAvailability>(MemberList.None);
    }
}
