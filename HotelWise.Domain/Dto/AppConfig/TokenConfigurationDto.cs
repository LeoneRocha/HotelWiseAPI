using HotelWise.Domain.Interfaces.AppConfig;

namespace HotelWise.Domain.Dto.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Security.TokenConfigurationDto.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SECURITY")]
    public class TokenConfigurationDto : HotelWise.Core.SDK.Security.TokenConfigurationDto, ITokenConfigurationDto
    {
    }
}
