using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de transporte de disponibilidade de quarto com grade de preços por dia da semana.
/// </summary>
public class RoomAvailabilityDto
{
    /// <summary>
    /// Identificador único do registro de disponibilidade.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador do quarto ao qual a disponibilidade pertence.
    /// </summary>
    public long RoomId { get; set; }

    /// <summary>
    /// Moeda de cotação dos valores (padrão: "USD").
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Data inicial do período de vigência da disponibilidade.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data final do período de vigência da disponibilidade.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Itens contendo a quantidade disponível, status e preço detalhado por dia da semana.
    /// </summary>
    public RoomPriceAndAvailabilityItem[] AvailabilityWithPrice { get; set; } = [];
}