using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Interfaces.AppConfig;

namespace HotelWise.Service.Security
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Security.TokenService.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public class TokenService : HotelWise.Core.SDK.Security.TokenService, ITokenService
    {
        public TokenService(TokenConfigurationDto configuration)
            : base(configuration)
        {
        }
    }
}
