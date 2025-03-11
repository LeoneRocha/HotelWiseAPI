using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class UserRepository : GenericRepositoryBase<User>, IUserRepository
    {
        public UserRepository(HotelWiseDbContextMysql context) : base(context)
        {
        }

        public async Task<User?> FindByEmail(string value)
        {
            // Método específico para buscar um usuário pelo e-mail
            return await _dataset
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email.ToLower().Trim().Equals(value.ToLower().Trim()));
        }

        public async Task<User?> FindByLogin(string login)
        {
            // Método específico para buscar um usuário pelo login
            return await _dataset
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Login.ToLower().Trim().Equals(login.ToLower().Trim()));
        }

        public async Task<bool> UserExists(string login)
        {
            // Método específico para verificar se o usuário existe pelo login
            return await _dataset.AnyAsync(x => x.Login.ToLower().Equals(login.ToLower()));
        }

        public async Task<User> RefreshUserInfo(User user)
        {
            // Atualizar informações do usuário no banco de dados
            var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return result;
            }

            return new User();
        }
    }
}
