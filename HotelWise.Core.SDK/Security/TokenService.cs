#if NET8_0_OR_GREATER
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Emissão e validação de tokens JWT.
/// </summary>
public class TokenService : ITokenService
{
    private readonly ITokenConfigurationDto _configuration;

    public TokenService(ITokenConfigurationDto configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

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

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

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
