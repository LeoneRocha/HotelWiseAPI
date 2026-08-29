namespace HotelWise.Domain.Enuns.Hotel;

/// <summary>
/// Condição de uso e estado operacional do quarto no hotel.
/// </summary>
public enum RoomStatus
{
    /// <summary>
    /// Quarto disponível e limpo, pronto para ocupação.
    /// </summary>
    Available = 1,

    /// <summary>
    /// Quarto atualmente ocupado por hóspedes.
    /// </summary>
    Occupied = 2,

    /// <summary>
    /// Quarto em processo de manutenção física ou reparo de instalações.
    /// </summary>
    Maintenance = 3,

    /// <summary>
    /// Quarto em processo de higienização e governança (limpeza).
    /// </summary>
    Cleaning = 4,

    /// <summary>
    /// Quarto indisponível para alocação por outros motivos.
    /// </summary>
    Unavailable = 5
}
