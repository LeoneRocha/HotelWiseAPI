using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base com nome e e-mail.
/// </summary>
public abstract class EntityBaseWithNameEmail : EntityBase
{
    [Column("Name", TypeName = "varchar(255)", Order = 2)]
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("Email", TypeName = "varchar(100)", Order = 3)]
    [MaxLength(100)]
    [Required]
    public string Email { get; set; } = string.Empty;
}
