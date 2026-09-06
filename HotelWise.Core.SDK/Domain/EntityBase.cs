using System.ComponentModel.DataAnnotations.Schema;
using HotelWise.Core.SDK.Abstractions;
using SmartCoreHub.Core.SDK.Domain.Entities.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Domain;

/// <summary>
/// Entidade base abstrata com identificador, flag de habilitação e auditoria temporal.
/// Casca HW sobre <see cref="LongEntityBase"/> (PR-D6).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Entities.Common.LongEntityBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Entities.Common.LongEntityBase em SmartCoreHub.Core.SDK.")]
public abstract class EntityBase : LongEntityBase, IEntityBase, IEntityBaseLog
{
    /// <summary>Alias legado de <see cref="LongEntityBase.IsActive"/>.</summary>
    [NotMapped]
    public bool Enable
    {
        get => IsActive;
        set => IsActive = value;
    }

    /// <summary>Alias legado de <see cref="LongEntityBase.CreatedAt"/>.</summary>
    [NotMapped]
    public DateTime CreatedDate
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }

    /// <summary>Alias legado de <see cref="LongEntityBase.UpdatedAt"/>.</summary>
    [NotMapped]
    public DateTime ModifyDate
    {
        get => UpdatedAt;
        set => UpdatedAt = value;
    }

    /// <summary>Data/hora do último acesso (extensão de produto HW).</summary>
    public DateTime LastAccessDate { get; set; }
}
