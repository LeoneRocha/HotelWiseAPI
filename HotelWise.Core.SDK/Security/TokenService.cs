#if NET8_0_OR_GREATER
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Serviço de emissão e validação de tokens JWT (access e refresh),
/// baseado em <see cref="ITokenConfigurationDto"/> (issuer, audience, secret e expiração).
/// Implementa <see cref="ITokenService"/> para uso em fluxos de autenticação da API.
/// </summary>
/// <example>
/// <code>
/// var access = tokenService.GenerateAccessToken(claims);
/// var refresh = tokenService.GenerateRefreshToken();
/// var principal = tokenService.GetPrincipalFromExpiredToken(expiredAccess);
/// </code>
/// </example>
public class TokenService : SmartCoreHub.Core.SDK.Service.Security.Ported.TokenService, ITokenService
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="TokenService"/> com as configurações de token.
    /// </summary>
    /// <param name="configuration">Configurações de JWT.</param>
    public TokenService(ITokenConfigurationDto configuration)
        : base(configuration)
    {
    }
}
#endif
