namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria temporal de entidade.
/// </summary>
public interface IEntityBaseLog
{
    DateTime CreatedDate { get; set; }
    DateTime ModifyDate { get; set; }
    DateTime LastAccessDate { get; set; }
}
