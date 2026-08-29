using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Mapper;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetUserAuthenticatedDto>();
        CreateMap<UserLoginDto, User>();
    }
}
