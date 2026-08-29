using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Mapper;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetUserAuthenticatedDto>()
            .ForMember(d => d.TokenAuth, opt => opt.Ignore())
            .ForMember(d => d.MedicalId, opt => opt.Ignore());
        CreateMap<UserLoginDto, User>(MemberList.None);
    }
}
