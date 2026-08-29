using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO base abstrato de entidade.
/// Implementa <see cref="IEntityDto"/> e adiciona a flag de habilitação,
/// servindo como raiz comum para DTOs de transferência entre API e serviços.
/// </summary>
public abstract class EntityDtoBase : IEntityDto
{
    /// <summary>
    /// Identificador único da entidade representada pelo DTO.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Indica se a entidade está habilitada (ativa) no sistema.
    /// </summary>
    public bool Enable { get; set; }
}
