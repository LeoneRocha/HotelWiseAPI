using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata que estende <see cref="EntityBase"/> com nome e e-mail obrigatórios.
/// Destinada a entidades de domínio que representam pessoas ou contatos
/// (por exemplo, usuários, hóspedes ou responsáveis).
/// </summary>
public abstract class EntityBaseWithNameEmail : EntityBase
{
    /// <summary>
    /// Nome completo ou razão social associado à entidade.
    /// Persistido como <c>varchar(255)</c>, obrigatório, com comprimento máximo de 255.
    /// </summary>
    [Column("Name", TypeName = "varchar(255)", Order = 2)]
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail associado à entidade.
    /// Persistido como <c>varchar(100)</c>, obrigatório, com comprimento máximo de 100.
    /// </summary>
    [Column("Email", TypeName = "varchar(100)", Order = 3)]
    [MaxLength(100)]
    [Required]
    public string Email { get; set; } = string.Empty;
}
