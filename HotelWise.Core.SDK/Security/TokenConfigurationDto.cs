using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Configuração de token JWT.
/// </summary>
public class TokenConfigurationDto : ITokenConfigurationDto
{
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int Minutes { get; set; }
    public int DaysToExpiry { get; set; }
}
