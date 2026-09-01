using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de serviço de aplicação orientado a DTOs.
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de entidade manipulado pelo serviço.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService em SmartCoreHub.Core.SDK.")]
public interface IGenericService<TDto> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService<TDto>
    where TDto : class
{
}
