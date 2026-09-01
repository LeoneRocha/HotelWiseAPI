
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de serviço de aplicação orientado a DTOs.
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de entidade manipulado pelo serviço.</typeparam>
public interface IGenericService<TDto> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService<TDto>
    where TDto : class
{
}
