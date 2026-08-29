using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository;

/// <summary>
/// Implementação concreta do repositório de usuários <see cref="User"/> utilizando EF Core e MySQL.
/// </summary>
public class UserRepository : GenericRepositoryBase<User, HotelWiseDbContextMysql>, IUserRepository
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">Instância do contexto EF Core.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public UserRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) : base(context, options)
    {
    }

    /// <summary>
    /// Busca um usuário no banco pelo seu endereço de e-mail sem rastreamento de entidades.
    /// </summary>
    /// <param name="value">Endereço de e-mail a pesquisar.</param>
    /// <returns>Entidade <see cref="User"/> correspondente ou <c>null</c> se não encontrada.</returns>
    public async Task<User?> FindByEmail(string value)
    {
        return await _dataset
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.ToLower().Trim().Equals(value.ToLower().Trim()));
    }

    /// <summary>
    /// Busca um usuário no banco pelo seu identificador de login sem rastreamento de entidades.
    /// </summary>
    /// <param name="login">Nome de login a pesquisar.</param>
    /// <returns>Entidade <see cref="User"/> correspondente ou <c>null</c> se não encontrada.</returns>
    public async Task<User?> FindByLogin(string login)
    {
        return await _dataset
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Login.ToLower().Trim().Equals(login.ToLower().Trim()));
    }

    /// <summary>
    /// Verifica se existe algum usuário cadastrado com o login informado.
    /// </summary>
    /// <param name="login">Nome de login a verificar.</param>
    /// <returns><c>true</c> se o usuário já existir no banco; caso contrário, <c>false</c>.</returns>
    public async Task<bool> UserExists(string login)
    {
        return await _dataset.AnyAsync(x => x.Login.ToLower().Equals(login.ToLower()));
    }

    /// <summary>
    /// Atualiza os valores da entidade de usuário preservando e sincronizando o estado no contexto.
    /// </summary>
    /// <param name="user">Entidade de usuário com os dados atualizados.</param>
    /// <returns>Instância do usuário atualizado e persistido.</returns>
    public async Task<User> RefreshUserInfo(User user)
    {
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
