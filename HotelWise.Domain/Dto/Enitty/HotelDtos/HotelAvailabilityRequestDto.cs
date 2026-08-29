namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de entrada para consulta de disponibilidade de quartos em um hotel para um determinado período.
/// </summary>
public class HotelAvailabilityRequestDto
{
    /// <summary>
    /// Identificador do hotel a ser consultado.
    /// </summary>
    public required long HotelId { get; set; }

    /// <summary>
    /// Data inicial do período de consulta de disponibilidade.
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// Data final do período de consulta de disponibilidade (opcional).
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Moeda de cotação dos valores (padrão: "USD").
    /// </summary>
    public required string Currency { get; set; } = "USD";
}
