using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

public class ReservationMappingProfile : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.RoomDetails, opt => opt.MapFrom(src => src.Room));
        CreateMap<ReservationDto, Reservation>(MemberList.None);
    }
}
