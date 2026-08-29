namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de entidade com identificador e flag de habilitação.
/// </summary>
public interface IEntityBase
{
    long Id { get; set; }
    bool Enable { get; set; }
}
