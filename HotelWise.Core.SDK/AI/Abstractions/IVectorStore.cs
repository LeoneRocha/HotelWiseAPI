using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de vector store genérico.
/// </summary>
public interface IVectorStoreAdapter<TVector> where TVector : class, IDataVector
{
    Task UpsertDataAsync(string nameCollection, TVector dataVector);
    Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors);
    Task<TVector?> GetByKey(string nameCollection, ulong dataKey);
    Task<bool> Exists(string nameCollection, ulong dataKey);
    Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria);
    Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding);
    Task DeleteAsync(string nameCollection, long dataKey);
}

/// <summary>
/// Fábrica de adapters de vector store.
/// </summary>
public interface IVectorStoreAdapterFactory
{
    IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector;
}

/// <summary>
/// Serviço de vector store tipado por entidade.
/// </summary>
public interface IVectorStoreService<TEntity>
{
    Task UpsertDataAsync(TEntity entity);
    Task UpsertDatasAsync(TEntity[] listEntity);
    Task<TEntity?> GetById(long dataKey);
    Task<ServiceResponse<TEntity[]>> VectorizedSearchAsync(SearchCriteria searchCriteria);
    Task<ServiceResponse<TEntity[]>> SearchAndAnalyzePluginAsync(string searchText);
    Task<float[]?> GenerateEmbeddingAsync(string text);
    Task DeleteAsync(long dataKey);
}
