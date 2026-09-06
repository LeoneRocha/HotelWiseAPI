#if NET8_0_OR_GREATER
using System.Security.Claims;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.Extensions.Configuration;
using SmartCoreHub.Core.SDK.Common.Attributes;
using SmartCoreHub.Core.SDK.Domain.DTOs.Entities;
using SmartCoreHub.Core.SDK.Infrastructure.Security;
using SmartCoreHub.Core.SDK.Service.Security;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Serviço de emissão JWT local composando <see cref="JwtAccessTokenService"/> (sem SCH Ported).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Security.JwtAccessTokenService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca HW sobre JwtAccessTokenService.")]
public class TokenService : ITokenService
{
    private readonly IJwtAccessTokenService _inner;

    /// <summary>Cria wrapper delegando ao serviço JWT canônico.</summary>
    public TokenService(IJwtAccessTokenService inner) =>
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));

    /// <summary>Construtor legado baseado em configuração de token.</summary>
    public TokenService(HotelWise.Core.SDK.Abstractions.ITokenConfigurationDto configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var domainConfig = configuration as TokenConfigurationDto ?? new TokenConfigurationDto
        {
            Audience = configuration.Audience,
            Issuer = configuration.Issuer,
            Secret = configuration.Secret,
            Minutes = configuration.Minutes,
            DaysToExpiry = configuration.DaysToExpiry
        };
        _inner = new JwtAccessTokenService(new SecurityTokenAdapterFactory(new ConfigurationBuilder().Build(), domainConfig));
    }

    /// <inheritdoc />
    public string GenerateAccessToken(IEnumerable<Claim> claims) => _inner.GenerateAccessToken(claims);

    /// <inheritdoc />
    public string GenerateRefreshToken() => _inner.GenerateRefreshToken();

    /// <inheritdoc />
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) => _inner.GetPrincipalFromExpiredToken(token);
}
#endif
