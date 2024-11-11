namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreAdapter<TVector>
    {
        Task UpsertDataAsync(string nameCollection, TVector[] vectors);
        Task<TVector?> GetById(string nameCollection, ulong dataKey);
        Task<TVector[]> SearchDatasAsync(string nameCollection, string searchText);
    }
}