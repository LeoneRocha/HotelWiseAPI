using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreService<TEntity>
    {
        Task UpsertHotelAsync(TEntity[] listEntity);
        Task<TEntity?> GetById(long hotelId);
        Task<TEntity[]> SearchHotelsAsync(string searchText);
        Task<float[]?> GenerateEmbeddingAsync(string text);
    }
}