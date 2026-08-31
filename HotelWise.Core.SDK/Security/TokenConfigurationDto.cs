using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// DTO de configuração de token JWT (audience, issuer, secret e prazos),
/// tipicamente preenchido via bind da seção <c>TokenConfigurations</c> do appsettings.
/// Implementa <see cref="ITokenConfigurationDto"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TokenConfigurationDto : ITokenConfigurationDto
{
    /// <summary>
    /// Audience esperada nos tokens emitidos.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Issuer dos tokens emitidos.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Chave secreta usada na assinatura simétrica do JWT.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Validade do access token em minutos.
    /// </summary>
    public int Minutes { get; set; }

    /// <summary>
    /// Validade do refresh token (ou sessão) em dias.
    /// </summary>
    public int DaysToExpiry { get; set; }
}
