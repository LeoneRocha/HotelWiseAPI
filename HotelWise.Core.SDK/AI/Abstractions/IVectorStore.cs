using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de vector store genérico para operações CRUD e busca semântica.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapter<TVector>
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter<TVector>
    where TVector : class, IDataVector
{
}

/// <summary>
/// Fábrica de adapters de vector store tipados por <see cref="IDataVector"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapterFactory
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory
{
}

/// <summary>
/// Serviço de vector store tipado por entidade de domínio.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade de domínio mapeada para o vector store.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreService<TEntity>
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService<TEntity>
{
}
