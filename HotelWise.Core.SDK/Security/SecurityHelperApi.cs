using System.Security.Claims;
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Extrai user id de ClaimsPrincipal (local, sem SCH Ported).
/// </summary>
[SdkWrappedSource(targetType: "HotelWise.Domain.Helpers.UserClaimsHelper", targetPackage: "HotelWise.Domain", description: "Casca local GetUserId sem SCH Ported.")]
public static class SecurityHelperApi
{
    private const string JwtNameIdClaim = "nameid";

    /// <summary>
    /// Lê NameId e depois NameIdentifier; retorna 0 se ausente.
    /// </summary>
    public static long GetUserIdApi(ClaimsPrincipal? user)
    {
        if (user == null)
        {
            return 0;
        }

        var nameId = user.FindFirst(JwtNameIdClaim)?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return long.TryParse(nameId, out var idUser) ? idUser : 0;
    }
}
