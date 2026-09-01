
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria temporal de entidade.
/// Expõe marcas de tempo de criação, última alteração e último acesso,
/// usadas para rastreabilidade e políticas de retenção/atividade.
/// </summary>
public interface IEntityBaseLog : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBaseLog
{
}
