namespace HotelWise.Domain.Model.HotelModels;

/// <summary>
/// Entidade de domínio que representa a vigência de disponibilidade e precificação de um quarto em um determinado período.
/// </summary>
public class RoomAvailability
{
    /// <summary>
    /// Identificador único da disponibilidade.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador do quarto correspondente.
    /// </summary>
    public long RoomId { get; set; }

    /// <summary>
    /// Matriz de preços e quantidades disponíveis categorizadas por dia da semana.
    /// </summary>
    public RoomPriceAndAvailabilityItem[] AvailabilityWithPrice { get; set; } = [];        

    /// <summary>
    /// Referência de navegação ao <see cref="Room"/> vinculado.
    /// </summary>
    public Room Room { get; set; } = null!;

    /// <summary>
    /// Moeda de cotação dos valores configurados (padrão: "USD").
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
}
