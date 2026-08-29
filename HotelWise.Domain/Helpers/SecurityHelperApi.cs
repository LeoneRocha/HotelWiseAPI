using System.Security.Claims;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Security.SecurityHelperApi.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public static class SecurityHelperApi
    {
        public static long GetUserIdApi(ClaimsPrincipal user) =>
            HotelWise.Core.SDK.Security.SecurityHelperApi.GetUserIdApi(user);
    }
}
