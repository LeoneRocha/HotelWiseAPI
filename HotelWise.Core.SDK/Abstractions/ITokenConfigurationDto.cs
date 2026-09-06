using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de configuração JWT — casca sobre Domain.DTOs.Entities (não Obsolete Abstractions).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.DTOs.Entities.ITokenConfigurationDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para Domain.DTOs.Entities.ITokenConfigurationDto.")]
public interface ITokenConfigurationDto : SmartCoreHub.Core.SDK.Domain.DTOs.Entities.ITokenConfigurationDto
{
}
