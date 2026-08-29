namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Dados de segurança para geração de token.
/// </summary>
public class SecurityDto
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string SecurityKeyConfig { get; set; } = string.Empty;
}
