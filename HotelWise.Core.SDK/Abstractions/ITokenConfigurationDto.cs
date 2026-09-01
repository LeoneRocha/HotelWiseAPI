using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de configuração utilizada na emissão e validação de tokens JWT.
/// Define audiência, emissor, segredo de assinatura e prazos de validade
/// consumidos pelos serviços de autenticação do SDK.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto em SmartCoreHub.Core.SDK.")]
public interface ITokenConfigurationDto : SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto
{
}
