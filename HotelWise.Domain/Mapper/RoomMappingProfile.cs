using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

public class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.RoomAvailabilities));
        CreateMap<RoomDto, Room>();
    }
}
