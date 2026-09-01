using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria por usuário com campos escalares portáveis.
/// Registra quem criou/alterou o registro e quando, sem acoplar navegações
/// à entidade de usuário de domínio — essas permanecem no host da aplicação.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog em SmartCoreHub.Core.SDK.")]
public interface IEntityFieldBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog
{
}
