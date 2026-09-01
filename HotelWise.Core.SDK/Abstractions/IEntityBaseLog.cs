using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria temporal de entidade.
/// Expõe marcas de tempo de criação, última alteração e último acesso,
/// usadas para rastreabilidade e políticas de retenção/atividade.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog em SmartCoreHub.Core.SDK.")]
public interface IEntityBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog
{
}
