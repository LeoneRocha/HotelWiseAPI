using System.Linq.Expressions;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de serviço de entidade (DTO).
/// </summary>
/// <typeparam name="TDto">Tipo do DTO.</typeparam>
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
