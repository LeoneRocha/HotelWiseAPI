using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO base de entidade.
/// </summary>
public abstract class EntityDtoBase : IEntityDto
{
    public long Id { get; set; }
    public bool Enable { get; set; }
}
