namespace HotelWise.Domain.Dto.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Security.TokenVO.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public class TokenVO : HotelWise.Core.SDK.Security.TokenVO
    {
        public TokenVO()
        {
        }

        public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
            : base(authenticated, created, expiration, accessToken, refreshToken)
        {
        }
    }
}
