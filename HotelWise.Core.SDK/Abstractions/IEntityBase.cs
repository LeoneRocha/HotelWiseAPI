namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de entidade de domínio com identificador e flag de habilitação.
/// Base comum para entidades persistidas que suportam ativação/desativação lógica.
/// </summary>
public interface IEntityBase
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// Indica se a entidade está habilitada (ativa) no sistema.
    /// </summary>
    bool Enable { get; set; }
}
