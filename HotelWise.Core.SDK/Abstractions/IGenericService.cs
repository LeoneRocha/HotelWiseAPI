using System.Linq.Expressions;

using SmartCoreHub.Core.SDK.Common.Attributes;
using SmartCoreHub.Core.SDK.Domain.DTOs.Common;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato DTO-first local (não herda SCH Obsolete Abstractions.IGenericService).
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de entidade manipulado pelo serviço.</typeparam>
[SdkWrappedSource(targetType: "HotelWise.Domain.Interfaces.Generic.IGenericDtoService`1", targetPackage: "HotelWise.Domain", description: "Casca local DTO-first alinhada a IGenericDtoService.")]
public interface IGenericService<TDto> where TDto : class
{
    /// <summary>Define o identificador do usuário autenticado no contexto do serviço.</summary>
    void SetUserId(long id);

    /// <summary>Obtém todos os registros do tipo <typeparamref name="TDto"/>.</summary>
    Task<List<TDto>> GetAllAsync();

    /// <summary>Obtém um registro pelo identificador.</summary>
    Task<TDto?> GetByIdAsync(long id);

    /// <summary>Busca registros que satisfaçam o predicado informado.</summary>
    Task<List<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);

    /// <summary>Cria um novo registro a partir do DTO informado.</summary>
    Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto);

    /// <summary>Insere em lote uma coleção de DTOs.</summary>
    Task AddRangeAsync(IEnumerable<TDto> entitiesDto);

    /// <summary>Atualiza um registro existente a partir do DTO informado.</summary>
    Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto);

    /// <summary>Atualiza em lote uma coleção de DTOs.</summary>
    Task UpdateRangeAsync(IEnumerable<TDto> entitiesDto);

    /// <summary>Exclui o registro identificado por <paramref name="id"/>.</summary>
    Task DeleteAsync(long id);

    /// <summary>Conta a quantidade total de registros.</summary>
    Task<int> CountAsync();

    /// <summary>Obtém uma página de registros com deslocamento e limite.</summary>
    Task<List<TDto>> FetchAsync(int offset, int limit);
}
