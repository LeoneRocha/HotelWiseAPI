using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de transporte de dados de reserva de quarto, com status, valores, períodos e detalhes do quarto associado.
/// </summary>
public class ReservationDto
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
    /// Data e hora de entrada (check-in).
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Data e hora de saída (check-out).
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Data e hora em que a reserva foi solicitada/efetuada.
    /// </summary>
    public DateTime ReservationDate { get; set; }

    /// <summary>
    /// Valor total cobrado pela reserva.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Código ISO da moeda utilizada (ex: USD, BRL).
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Situação atual da reserva (Confirmada, Cancelada, Pendente).
    /// </summary>
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    /// <summary>
    /// Dados detalhados do quarto associado à reserva.
    /// </summary>
    public RoomDto? RoomDetails { get; set; }
}