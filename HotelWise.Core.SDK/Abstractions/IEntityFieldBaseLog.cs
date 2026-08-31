namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria por usuário com campos escalares portáveis.
/// Registra quem criou/alterou o registro e quando, sem acoplar navegações
/// à entidade de usuário de domínio — essas permanecem no host da aplicação.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IEntityFieldBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityFieldBaseLog
{
}
