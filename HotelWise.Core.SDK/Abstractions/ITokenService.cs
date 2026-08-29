using System.Security.Claims;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de serviço responsável pela emissão, renovação e validação de tokens de autenticação (JWT).
/// Abstrai a geração de access tokens a partir de claims, a emissão de refresh tokens
/// e a recuperação do principal de segurança a partir de um token expirado.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um access token JWT contendo as claims informadas.
    /// </summary>
    /// <param name="claims">Coleção de claims a incluir no token (identidade, papéis, etc.).</param>
    /// <returns>String do access token JWT gerado.</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Gera um refresh token opaco para renovação de sessão sem reautenticação completa.
    /// </summary>
    /// <returns>String do refresh token gerado.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Obtém o <see cref="ClaimsPrincipal"/> a partir de um access token expirado,
    /// permitindo renovar a sessão sem exigir novas credenciais, desde que a assinatura seja válida.
    /// </summary>
    /// <param name="token">Access token JWT expirado, porém com assinatura válida.</param>
    /// <returns>Principal de segurança reconstruído a partir das claims do token.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
