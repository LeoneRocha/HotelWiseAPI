namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de serviço de aplicação orientado a DTOs.
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de entidade manipulado pelo serviço.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IGenericService<TDto> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService<TDto>
    where TDto : class
{
}
