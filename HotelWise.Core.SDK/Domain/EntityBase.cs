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
public abstract class EntityBase : IEntityBase, IEntityBaseLog
{
    /// <summary>
    /// Identificador único da entidade, gerado automaticamente pelo banco de dados.
    /// </summary>
    [Column("Id", Order = 0)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// Indica se a entidade está habilitada (ativa). Valor padrão: <c>true</c>.
    /// </summary>
    [Column("Enable", Order = 1)]
    [DefaultValue(true)]
    public bool Enable { get; set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    [Column("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Data e hora da última alteração do registro.
    /// </summary>
    [Column("ModifyDate")]
    public DateTime ModifyDate { get; set; }

    /// <summary>
    /// Data e hora do último acesso ao registro.
    /// </summary>
    [Column("LastAccessDate")]
    public DateTime LastAccessDate { get; set; }
}
