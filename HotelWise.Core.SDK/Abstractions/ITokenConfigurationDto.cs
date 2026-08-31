namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de configuração utilizada na emissão e validação de tokens JWT.
/// Define audiência, emissor, segredo de assinatura e prazos de validade
/// consumidos pelos serviços de autenticação do SDK.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface ITokenConfigurationDto
{
    /// <summary>
    /// Audiência (claim <c>aud</c>) esperada no token JWT.
    /// </summary>
    string Audience { get; set; }

    /// <summary>
    /// Emissor (claim <c>iss</c>) do token JWT.
    /// </summary>
    string Issuer { get; set; }

    /// <summary>
    /// Segredo simétrico usado para assinar e validar o token JWT.
    /// </summary>
    string Secret { get; set; }

    /// <summary>
    /// Tempo de validade do access token, em minutos.
    /// </summary>
    int Minutes { get; set; }

    /// <summary>
    /// Quantidade de dias até a expiração do refresh token (ou sessão associada).
    /// </summary>
    int DaysToExpiry { get; set; }
}
