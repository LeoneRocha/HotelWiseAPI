namespace HotelWise.Domain.Enuns.Hotel;

/// <summary>
/// Estados do ciclo de vida de uma reserva hoteleira.
/// </summary>
public enum ReservationStatus
{
    /// <summary>
    /// Reserva confirmada e garantida para o hóspede.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Reserva cancelada pelo hóspede ou pelo estabelecimento.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Reserva solicitada e aguardando processamento de pagamento ou aprovação.
    /// </summary>
    Pending = 3
}
