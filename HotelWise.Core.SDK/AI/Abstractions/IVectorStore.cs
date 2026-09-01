using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de vector store genérico para operações CRUD e busca semântica.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter em SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapter<TVector>
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter<TVector>
    where TVector : class, IDataVector
{
}

/// <summary>
/// Fábrica de adapters de vector store tipados por <see cref="IDataVector"/>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory em SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapterFactory
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory
{
}

/// <summary>
/// Serviço de vector store tipado por entidade de domínio.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade de domínio mapeada para o vector store.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService em SmartCoreHub.Core.SDK.")]
public interface IVectorStoreService<TEntity>
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService<TEntity>
{
}
