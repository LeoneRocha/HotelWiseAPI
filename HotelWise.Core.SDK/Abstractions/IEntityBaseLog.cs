namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria temporal de entidade.
/// Expõe marcas de tempo de criação, última alteração e último acesso,
/// usadas para rastreabilidade e políticas de retenção/atividade.
/// </summary>
public interface IEntityBaseLog
{
    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    /// Data e hora da última alteração do registro.
    /// </summary>
    DateTime ModifyDate { get; set; }

    /// <summary>
    /// Data e hora do último acesso ao registro.
    /// </summary>
    DateTime LastAccessDate { get; set; }
}
