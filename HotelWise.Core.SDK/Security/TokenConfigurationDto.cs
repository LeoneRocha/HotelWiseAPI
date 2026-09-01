using HotelWise.Core.SDK.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// DTO de configuração de token JWT (audience, issuer, secret e prazos),
/// tipicamente preenchido via bind da seção <c>TokenConfigurations</c> do appsettings.
/// Implementa <see cref="ITokenConfigurationDto"/>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto em SmartCoreHub.Core.SDK.")]
public class TokenConfigurationDto : SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto, ITokenConfigurationDto
{

}
