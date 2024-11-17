using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IUserRepository  
    {
        Task<User?> FindByEmail(string value);
        Task<User?> FindByLogin(string login);
        Task<User> RefreshUserInfo(User user);
        Task<bool> UserExists(string login);
    }
}
