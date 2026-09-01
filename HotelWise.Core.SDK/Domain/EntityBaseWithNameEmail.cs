using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata que estende <see cref="EntityBase"/> com nome e e-mail obrigatórios.
/// Destinada a entidades de domínio que representam pessoas ou contatos
/// (por exemplo, usuários, hóspedes ou responsáveis).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail em SmartCoreHub.Core.SDK.")]
public abstract class EntityBaseWithNameEmail : SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail
{

}
