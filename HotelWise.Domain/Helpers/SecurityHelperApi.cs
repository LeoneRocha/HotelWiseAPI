using System.Security.Claims;

namespace HotelWise.Domain.Helpers
{
    public static class SecurityHelperApi
    {
        public static long GetUserIdApi(ClaimsPrincipal user)
        {
            long idUserResult = 0;
            long idUser;
            if (user != null && long.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out idUser))
            {
                idUserResult = idUser;
            }
            return idUserResult;
        }
    }
}
