using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Model.HotelModels;

/// <summary>
/// Estrutura de valor que define o preço diário, estoque de vagas e status de disponibilidade por dia da semana.
/// </summary>
public class RoomPriceAndAvailabilityItem
{
    /// <summary>
    /// Dia da semana correspondente à regra de tarifação (Sunday, Monday, etc.).
    /// </summary>
    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>
    /// Valor monetário da diária para o dia da semana configurado.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantidade de quartos disponíveis para alocação neste dia da semana.
    /// </summary>
    public int QuantityAvailable { get; set; }

    /// <summary>
    /// Moeda de cotação do preço (ex: "USD", "BRL").
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Status operacional de disponibilidade da vaga (Available, Reserved, Blocked).
    /// </summary>
    public RoomAvailabilityStatus Status { get; set; } = RoomAvailabilityStatus.Available;
}