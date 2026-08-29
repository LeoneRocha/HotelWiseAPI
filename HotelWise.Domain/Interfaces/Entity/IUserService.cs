using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Interfaces.Entity;

/// <summary>
/// Contrato de serviço para operações de autenticação e gerenciamento de usuários.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Realiza a autenticação do usuário por login e senha, gerando o token JWT e o refresh token.
    /// </summary>
    /// <param name="login">Nome de login ou e-mail do usuário.</param>
    /// <param name="password">Senha em texto simples a ser validada.</param>
    /// <returns>Resposta com o DTO do usuário autenticado e suas credenciais de token, ou mensagem de falha.</returns>
    Task<ServiceResponse<GetUserAuthenticatedDto>> Login(string login, string password);
}
