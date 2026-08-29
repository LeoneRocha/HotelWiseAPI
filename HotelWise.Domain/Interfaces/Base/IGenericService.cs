using HotelWise.Domain.Dto;
using System.Linq.Expressions;

namespace HotelWise.Domain.Interfaces.Base
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Shim mantém assinaturas com <see cref="ServiceResponse{T}"/> do host para zero regressão
    /// (Task&lt;T&gt; é invariante; herdar o contrato Core quebraria implementadores existentes).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.IGenericService<TDto>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SERVICE")]
    public interface IGenericService<TDto> where TDto : class
    {
        void SetUserId(long id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(long id);
        Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);
        Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto);
        Task AddRangeAsync(IEnumerable<TDto> entitiesDto);
        Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto);
        Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto);
        Task DeleteAsync(long id);
        Task<int> CountAsync();
        Task<List<TDto>> FetchAsync(int offset, int limit);
    }
}
