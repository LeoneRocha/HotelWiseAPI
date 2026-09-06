using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata que estende <see cref="EntityBase"/> com nome e e-mail obrigatórios
/// (local sobre LongEntityBase).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Entities.Common.LongEntityBase", targetPackage: "SmartCoreHub.Core.SDK", description: "EntityBase + Name/Email locais sobre LongEntityBase.")]
public abstract class EntityBaseWithNameEmail : EntityBase
{
    /// <summary>Nome da entidade (obrigatório).</summary>
    [Column("Name", TypeName = "varchar(255)", Order = 2)]
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>E-mail da entidade (obrigatório).</summary>
    [Column("Email", TypeName = "varchar(100)", Order = 3)]
    [MaxLength(100)]
    [Required]
    public string Email { get; set; } = string.Empty;
}
