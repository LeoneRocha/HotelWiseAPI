using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Interfaces
{
    public interface IUserService  
    {
        Task<ServiceResponse<GetUserAuthenticatedDto>> Login(string login, string password);
    }
}
