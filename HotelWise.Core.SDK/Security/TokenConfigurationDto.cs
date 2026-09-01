using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// DTO de configuração de token JWT (audience, issuer, secret e prazos),
/// tipicamente preenchido via bind da seção <c>TokenConfigurations</c> do appsettings.
/// Implementa <see cref="ITokenConfigurationDto"/>.
/// </summary>
public class TokenConfigurationDto : SmartCoreHub.Core.SDK.Common.Security.TokenConfigurationDto, ITokenConfigurationDto
{

}
