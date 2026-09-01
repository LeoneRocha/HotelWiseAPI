
namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Value object imutável (após construção) que representa o resultado de autenticação:
/// flag de autenticado, timestamps e pares access/refresh token.
/// </summary>
public class TokenVO : SmartCoreHub.Core.SDK.Common.Security.TokenVO
{
    /// <summary>Inicializa uma nova instância vazia de <see cref="TokenVO"/>.</summary>
    public TokenVO()
    {
    }

    /// <summary>Inicializa uma nova instância de <see cref="TokenVO"/> com todos os parâmetros de autenticação.</summary>
    /// <param name="authenticated">Indica se a autenticação foi bem-sucedida.</param>
    /// <param name="created">Data/hora de criação do token.</param>
    /// <param name="expiration">Data/hora de expiração do token.</param>
    /// <param name="accessToken">Token JWT de acesso.</param>
    /// <param name="refreshToken">Token de refresh.</param>
    public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
        : base(authenticated, created, expiration, accessToken, refreshToken)
    {
    }
}
