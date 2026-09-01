
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de DTO de entidade.
/// Garante a presença do identificador numérico usado em transferência de dados
/// entre camadas de API, serviço e persistência.
/// </summary>
public interface IEntityDto : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityDto
{
}
