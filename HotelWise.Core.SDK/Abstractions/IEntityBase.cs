using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de entidade de domínio com identificador e flag de habilitação.
/// Base comum para entidades persistidas que suportam ativação/desativação lógica.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBase em SmartCoreHub.Core.SDK.")]
public interface IEntityBase : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBase
{
}
