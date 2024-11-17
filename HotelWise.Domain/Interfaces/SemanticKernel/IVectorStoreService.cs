namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreService<TEntity>
    {
        Task UpsertDataAsync(TEntity hotel);
        Task UpsertDatasAsync(TEntity[] listEntity);
        Task<TEntity?> GetById(long dataKey);
        Task<TEntity[]> SearchDatasAsync(string searchText);
        Task<float[]?> GenerateEmbeddingAsync(string text);
        Task DeleteAsync(long dataKey);
    }
}