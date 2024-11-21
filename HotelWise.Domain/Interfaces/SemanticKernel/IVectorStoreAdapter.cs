using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.IA;

namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreAdapter<TVector> where TVector : IDataVector
    {
        Task UpsertDataAsync(string nameCollection, TVector dataVector);
        Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors);

        Task<TVector?> GetByKey(string nameCollection, ulong dataKey);

        Task<bool> Exists(string nameCollection, ulong dataKey);

        Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria);

        Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding);
        Task DeleteAsync(string nameCollection, long dataKey);
    }
}