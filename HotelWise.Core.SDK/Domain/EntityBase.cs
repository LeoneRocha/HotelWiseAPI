using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata com identificador, flag de habilitação e auditoria temporal.
/// Implementa <see cref="IEntityBase"/> e <see cref="IEntityBaseLog"/> e serve como
/// raiz comum para entidades de domínio persistidas via EF Core no SDK.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class EntityBase : SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBase, IEntityBase, IEntityBaseLog
{

}
