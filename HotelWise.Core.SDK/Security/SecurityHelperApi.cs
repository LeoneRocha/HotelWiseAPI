using System.Security.Claims;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Extração de claims de usuário em APIs.
/// </summary>
public static class SecurityHelperApi
{
    public static long GetUserIdApi(ClaimsPrincipal user)
    {
        long idUserResult = 0;
        if (user != null && long.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out long idUser))
        {
            idUserResult = idUser;
        }
        return idUserResult;
    }
}
