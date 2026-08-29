namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria por usuário (campos escalares portáveis).
/// Navegações para entidade de usuário de domínio permanecem no host.
/// </summary>
public interface IEntityFieldBaseLog
{
    long? CreatedUserId { get; set; }
    long? ModifyUserId { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime ModifyDate { get; set; }
}
