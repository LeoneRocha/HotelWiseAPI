using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HotelWiseDbContextMysql _context;
        private readonly DbContextOptions<HotelWiseDbContextMysql> _options;

        public UserRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        {
            _context = context;
            _options = options;

            if (context == null)
            {
                _context = CreateContext();
            }
        }

        private HotelWiseDbContextMysql CreateContext()
        {
            return new HotelWiseDbContextMysql(_options);
        }
        public async Task<User?> FindByEmail(string value)
        {
            User? userResult = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email.ToLower().Trim().Equals(value.ToLower().Trim()));

            return userResult;
        }

        public async Task<User?> FindByLogin(string login)
        {
            User? userResult = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Login.ToLower().Trim().Equals(login.ToLower().Trim()));

            return userResult;
        }

        public async Task<bool> UserExists(string login)
        {
            if (await _context.Users.AnyAsync(x => x.Login.ToLower().Equals(login.ToLower())))
            {
                return true;
            }
            return false;
        }

        public async Task<User> RefreshUserInfo(User user)
        {
            if (!(await _context.Users.AnyAsync(u => u.Id.Equals(user.Id)))) return new User();

            var result = await _context.Users.SingleOrDefaultAsync(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                _context.Users.Entry(result).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return result;
            }
            return new User();
        }
    }
}
