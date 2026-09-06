using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de usuários <see cref="User"/> utilizando EF Core e MySQL.
/// </summary>
public class UserRepository : GenericRepository<User, HotelWiseDbContextMysql>, IUserRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public UserRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options)
        : base(context, NullAppLogger.Instance, options)
    {
    }

    /// <inheritdoc />
    public async Task<User?> FindByEmail(string value)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.ToLower().Trim().Equals(value.ToLower().Trim()));
    }

    /// <inheritdoc />
    public async Task<User?> FindByLogin(string login)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Login.ToLower().Trim().Equals(login.ToLower().Trim()));
    }

    /// <inheritdoc />
    public async Task<bool> UserExists(string login)
    {
        return await _dbSet.AnyAsync(x => x.Login.ToLower().Equals(login.ToLower()));
    }

    /// <inheritdoc />
    public async Task<User> RefreshUserInfo(User user)
    {
        var result = await _dbSet.SingleOrDefaultAsync(p => p.Id.Equals(user.Id));
        if (result != null)
        {
            _context.Entry(result).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return result;
        }

        return new User();
    }
}
