using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata que estende <see cref="EntityBase"/> com nome e e-mail obrigatórios.
/// Destinada a entidades de domínio que representam pessoas ou contatos
/// (por exemplo, usuários, hóspedes ou responsáveis).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class EntityBaseWithNameEmail : SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail
{

}
