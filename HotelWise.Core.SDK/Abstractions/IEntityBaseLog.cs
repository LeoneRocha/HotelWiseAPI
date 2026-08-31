namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria temporal de entidade.
/// Expõe marcas de tempo de criação, última alteração e último acesso,
/// usadas para rastreabilidade e políticas de retenção/atividade.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IEntityBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog
{
}
