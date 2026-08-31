#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Base de dado vetorial com atributos de vector store.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class DataVectorBase : SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase, IDataVector
{
}
#endif
