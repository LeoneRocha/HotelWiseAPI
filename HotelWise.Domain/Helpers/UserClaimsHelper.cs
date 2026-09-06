using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelWise.Domain.Helpers;

/// <summary>
/// Extrai o user id de <see cref="ClaimsPrincipal"/> (JWT NameId / NameIdentifier).
/// </summary>
public static class UserClaimsHelper
{
    /// <summary>
    /// Lê <see cref="JwtRegisteredClaimNames.NameId"/> e depois <see cref="ClaimTypes.NameIdentifier"/>.
    /// Retorna 0 se ausente ou inválido.
    /// </summary>
    public static long GetUserId(ClaimsPrincipal? user)
    {
        if (user == null)
        {
            return 0;
        }

        var nameId = user.FindFirst(JwtRegisteredClaimNames.NameId)?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return long.TryParse(nameId, out var idUser) ? idUser : 0;
    }
}
