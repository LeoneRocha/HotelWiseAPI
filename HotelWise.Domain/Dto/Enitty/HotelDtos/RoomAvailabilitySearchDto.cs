namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de entrada com critérios de busca de disponibilidade de quartos por hotel e intervalo de datas.
/// </summary>
public class RoomAvailabilitySearchDto
{
    /// <summary>
    /// Identificador do hotel a pesquisar.
    /// </summary>
    public required long HotelId { get; set; }

    /// <summary>
    /// Data inicial do período de busca.
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// Data final opcional do período de busca.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Código da moeda utilizada na cotação (padrão: "USD").
    /// </summary>
    public required string Currency { get; set; } = "USD";
}
