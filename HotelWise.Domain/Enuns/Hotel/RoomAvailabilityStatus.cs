namespace HotelWise.Domain.Enuns.Hotel;

/// <summary>
/// Status de disponibilidade operacional de um quarto para uma data ou período específico.
/// </summary>
public enum RoomAvailabilityStatus
{
    /// <summary>
    /// Quarto disponível e apto para novas reservas.
    /// </summary>
    Available = 1,

    /// <summary>
    /// Quarto já reservado no período correspondente.
    /// </summary>
    Reserved = 2,

    /// <summary>
    /// Quarto bloqueado para manutenção, reforma ou restrições administrativas.
    /// </summary>
    Blocked = 3
}
