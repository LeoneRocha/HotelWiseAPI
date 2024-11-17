using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region USER 
            CreateMap<User, GetUserAuthenticatedDto>();
            CreateMap<UserLoginDto, User>();
            #endregion USER 
        }
    }
}
