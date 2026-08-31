using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// DTO de configuração de token JWT (audience, issuer, secret e prazos),
/// tipicamente preenchido via bind da seção <c>TokenConfigurations</c> do appsettings.
/// Implementa <see cref="ITokenConfigurationDto"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TokenConfigurationDto : SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto, ITokenConfigurationDto
{

}
