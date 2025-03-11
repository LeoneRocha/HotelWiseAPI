using HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> FindByEmail(string value);
        Task<User?> FindByLogin(string login);
        Task<bool> UserExists(string login);
        Task<User> RefreshUserInfo(User user);
    }
}
