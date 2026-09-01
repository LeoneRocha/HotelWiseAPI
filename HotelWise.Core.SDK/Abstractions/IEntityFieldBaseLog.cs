
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria por usuário com campos escalares portáveis.
/// Registra quem criou/alterou o registro e quando, sem acoplar navegações
/// à entidade de usuário de domínio — essas permanecem no host da aplicação.
/// </summary>
public interface IEntityFieldBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog
{
}
