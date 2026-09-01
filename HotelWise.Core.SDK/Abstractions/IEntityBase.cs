
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de entidade de domínio com identificador e flag de habilitação.
/// Base comum para entidades persistidas que suportam ativação/desativação lógica.
/// </summary>
public interface IEntityBase : SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBase
{
}
