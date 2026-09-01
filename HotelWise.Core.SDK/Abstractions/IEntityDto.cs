using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de DTO de entidade.
/// Garante a presença do identificador numérico usado em transferência de dados
/// entre camadas de API, serviço e persistência.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto em SmartCoreHub.Core.SDK.")]
public interface IEntityDto : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto
{
}
