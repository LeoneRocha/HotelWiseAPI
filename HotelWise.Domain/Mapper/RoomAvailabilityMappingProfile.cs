using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

public class RoomAvailabilityMappingProfile : Profile
{
    public RoomAvailabilityMappingProfile()
    {
        CreateMap<RoomAvailability, RoomAvailabilityDto>()
            .ForMember(dest => dest.AvailabilityWithPrice, opt => opt.MapFrom(src => src.AvailabilityWithPrice));
        CreateMap<RoomAvailabilityDto, RoomAvailability>();
    }
}
