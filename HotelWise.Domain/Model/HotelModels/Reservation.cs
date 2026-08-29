using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Model.HotelModels;

/// <summary>
/// Entidade de domínio que representa a reserva de um quarto por um período específico.
/// </summary>
public class Reservation
{
    /// <summary>
    /// Identificador único da reserva.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador do quarto reservado.
    /// </summary>
    public long RoomId { get; set; }

    /// <summary>
    /// Identificador do usuário que realizou a reserva (opcional).
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Data de entrada (check-in) prevista na reserva.
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Data de saída (check-out) prevista na reserva.
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Data e hora da criação da reserva.
    /// </summary>
    public DateTime ReservationDate { get; set; }

    /// <summary>
    /// Valor monetário total cobrado pela estadia.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Código da moeda utilizada na transação (padrão: "USD").
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Status operacional da reserva (Confirmada, Cancelada, Pendente).
    /// </summary>
    public ReservationStatus Status { get; set; }

    /// <summary>
    /// Referência de navegação para a entidade <see cref="Room"/> reservada.
    /// </summary>
    public Room Room { get; set; } = null!;
}
