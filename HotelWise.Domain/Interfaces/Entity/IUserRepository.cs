using HotelWise.Core.SDK.Abstractions;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity;

/// <summary>
/// Contrato de repositório específico para operações de persistência e consulta de usuários do sistema.
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Localiza um usuário pelo seu endereço de e-mail.
    /// </summary>
    /// <param name="value">Endereço de e-mail a pesquisar.</param>
    /// <returns>Instância de <see cref="User"/> se encontrado; caso contrário, <c>null</c>.</returns>
    Task<User?> FindByEmail(string value);

    /// <summary>
    /// Localiza um usuário pelo seu nome de login.
    /// </summary>
    /// <param name="login">Nome de login do usuário.</param>
    /// <returns>Instância de <see cref="User"/> se encontrado; caso contrário, <c>null</c>.</returns>
    Task<User?> FindByLogin(string login);

    /// <summary>
    /// Verifica se já existe um usuário cadastrado com o login informado.
    /// </summary>
    /// <param name="login">Nome de login a verificar.</param>
    /// <returns><c>true</c> se o usuário já existe; caso contrário, <c>false</c>.</returns>
    Task<bool> UserExists(string login);

    /// <summary>
    /// Atualiza as informações de token e auditoria do usuário no banco de dados.
    /// </summary>
    /// <param name="user">Entidade de usuário com os dados atualizados.</param>
    /// <returns>Usuário atualizado.</returns>
    Task<User> RefreshUserInfo(User user);
}
