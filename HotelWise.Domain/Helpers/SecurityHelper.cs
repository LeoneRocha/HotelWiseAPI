using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Security.SecurityHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public static class SecurityHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) =>
            HotelWise.Core.SDK.Security.SecurityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) =>
            HotelWise.Core.SDK.Security.SecurityHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

        public static string CreateToken(SecurityDto secVo) =>
            HotelWise.Core.SDK.Security.SecurityHelper.CreateToken(secVo);

        public static bool IsBase64String(string base64) =>
            HotelWise.Core.SDK.Security.SecurityHelper.IsBase64String(base64);
    }
}
