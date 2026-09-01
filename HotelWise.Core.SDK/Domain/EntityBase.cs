using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelWise.Core.SDK.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata com identificador, flag de habilitação e auditoria temporal.
/// Implementa <see cref="IEntityBase"/> e <see cref="IEntityBaseLog"/> e serve como
/// raiz comum para entidades de domínio persistidas via EF Core no SDK.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBase em SmartCoreHub.Core.SDK.")]
public abstract class EntityBase : SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBase, IEntityBase, IEntityBaseLog
{

}
