using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
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

            #region Hotel 
            CreateMap<HotelDto, Hotel>();
            CreateMap<Hotel, HotelDto>();
            
            
            CreateMap<Hotel, HotelVector>();
            CreateMap<HotelVector, Hotel>();
            #endregion Hotel 
        }
    }
}
