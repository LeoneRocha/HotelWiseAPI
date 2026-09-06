using System.Security.Claims;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato local de emissão JWT (mesma superfície do legado, sem herdar SCH Obsolete).
/// Preferir <c>IJwtAccessTokenService</c> nos hosts.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Security.IJwtAccessTokenService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca local compatível com IJwtAccessTokenService (3 métodos).")]
public interface ITokenService
{
    /// <summary>Gera um access token JWT contendo as claims informadas.</summary>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>Gera um refresh token opaco para renovação de sessão.</summary>
    string GenerateRefreshToken();

    /// <summary>Obtém o <see cref="ClaimsPrincipal"/> a partir de um access token expirado.</summary>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
