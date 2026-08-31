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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.Security.Ported.TokenService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TokenService : ITokenService
{
    /// <summary>
    /// Configuração JWT (secret, issuer, audience e minutos de validade).
    /// </summary>
    private readonly ITokenConfigurationDto _configuration;

    /// <summary>
    /// Inicializa o serviço com a configuração de token.
    /// </summary>
    /// <param name="configuration">Configuração JWT; não pode ser nula.</param>
    /// <exception cref="ArgumentNullException">Quando <paramref name="configuration"/> é nula.</exception>
    public TokenService(ITokenConfigurationDto configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Gera um access token JWT assinado (HMAC-SHA512) com as claims informadas.
    /// </summary>
    /// <param name="claims">Claims a incluir no token.</param>
    /// <returns>Token JWT serializado.</returns>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        string secretKey = _configuration.Secret;
        var signinCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            SecurityAlgorithms.HmacSha512);

        var options = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_configuration.Minutes),
            signingCredentials: signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(options);
    }

    /// <summary>
    /// Gera um refresh token aleatório (32 bytes em Base64).
    /// </summary>
    /// <returns>String Base64 do refresh token.</returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Extrai o <see cref="ClaimsPrincipal"/> de um access token expirado,
    /// validando a assinatura mas ignorando lifetime (para fluxo de refresh).
    /// </summary>
    /// <param name="token">Access token JWT (possivelmente expirado).</param>
    /// <returns>Principal com as claims do token.</returns>
    /// <exception cref="SecurityTokenException">Quando o algoritmo ou token é inválido.</exception>
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCulture))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }
}
#endif
