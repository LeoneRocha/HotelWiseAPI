namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de DTO de entidade.
/// Garante a presença do identificador numérico usado em transferência de dados
/// entre camadas de API, serviço e persistência.
/// </summary>
public interface IEntityDto
{
    /// <summary>
    /// Identificador único da entidade representada pelo DTO.
    /// </summary>
    long Id { get; set; }
}
