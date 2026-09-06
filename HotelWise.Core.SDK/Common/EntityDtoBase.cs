using HotelWise.Core.SDK.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO base abstrato de entidade.
/// Implementa <see cref="IEntityDto"/> e adiciona a flag de habilitação,
/// servindo como raiz comum para DTOs de transferência entre API e serviços.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.EntityDtoBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.EntityDtoBase em SmartCoreHub.Core.SDK.")]
public abstract class EntityDtoBase : SmartCoreHub.Core.SDK.Common.EntityDtoBase, IEntityDto
{

}
