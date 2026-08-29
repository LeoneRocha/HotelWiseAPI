using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

/// <summary>
/// Perfil AutoMapper para mapeamento entre a entidade de domínio <see cref="Reservation"/> e o DTO <see cref="ReservationDto"/>.
/// </summary>
public class ReservationMappingProfile : Profile
{
    /// <summary>
    /// Configura o mapeamento de reservas incluindo os detalhes do quarto associado.
    /// </summary>
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.RoomDetails, opt => opt.MapFrom(src => src.Room));
        CreateMap<ReservationDto, Reservation>(MemberList.None);
    }
}
