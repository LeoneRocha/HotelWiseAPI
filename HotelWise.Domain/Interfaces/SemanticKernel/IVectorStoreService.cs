using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreService
    {
        Task UpsertHotelAsync(Hotel[] hotels);
        Task<Hotel?> GetById(long hotelId);
        Task<Hotel[]> SearchHotelsAsync(string searchText);

        Task<decimal[]?> GenerateEmbeddingAsync(string text);
    } 
}