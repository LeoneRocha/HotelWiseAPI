namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de DTO de entidade.
/// Garante a presença do identificador numérico usado em transferência de dados
/// entre camadas de API, serviço e persistência.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IEntityDto : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto
{
}
