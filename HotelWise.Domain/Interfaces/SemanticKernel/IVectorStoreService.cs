using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;

namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IVectorStoreService<TEntity>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
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
}
