using System.Security.Claims;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de serviço de emissão e validação de tokens.
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
