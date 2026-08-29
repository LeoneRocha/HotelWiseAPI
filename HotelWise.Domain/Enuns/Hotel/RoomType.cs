namespace HotelWise.Domain.Enuns.Hotel;

/// <summary>
/// Tipos de acomodação e categorias de quartos oferecidos pelos hotéis.
/// </summary>
public enum RoomType
{
    /// <summary>
    /// Quarto Individual para um único ocupante.
    /// </summary>
    Single = 1,

    /// <summary>
    /// Quarto Duplo com cama de casal ou duas camas de solteiro.
    /// </summary>
    Double = 2,

    /// <summary>
    /// Suíte com múltiplos cômodos e espaço ampliado.
    /// </summary>
    Suite = 3,

    /// <summary>
    /// Quarto planejado para acomodação de famílias.
    /// </summary>
    Family = 4,

    /// <summary>
    /// Quarto Deluxe com acabamentos de luxo e serviços diferenciados.
    /// </summary>
    Deluxe = 5
}
