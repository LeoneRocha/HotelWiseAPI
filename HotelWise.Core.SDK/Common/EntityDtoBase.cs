using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO base abstrato de entidade.
/// Implementa <see cref="IEntityDto"/> e adiciona a flag de habilitação,
/// servindo como raiz comum para DTOs de transferência entre API e serviços.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.EntityDtoBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class EntityDtoBase : SmartCoreHub.Core.SDK.Common.EntityDtoBase, IEntityDto
{

}
