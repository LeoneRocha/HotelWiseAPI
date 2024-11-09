using HotelWise.Domain.Dto.SemanticKernel;

namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreAdapter
    {
        Task UpsertHotelAsync(HotelVector[] hotels);
        Task<HotelVector?> GetById(ulong hotelId);
        Task<HotelVector[]> SearchHotelsAsync(string searchText);

        Task<ReadOnlyMemory<float>?> GenerateEmbeddingAsync(string text);
    }
}