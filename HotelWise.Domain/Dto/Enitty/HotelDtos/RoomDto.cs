using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de transferência e resposta com informações completas do quarto e sua lista de disponibilidades.
/// </summary>
public class RoomDto
{
    /// <summary>
    /// Identificador único do quarto.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador do hotel ao qual o quarto pertence.
    /// </summary>
    public long HotelId { get; set; }

    /// <summary>
    /// Classificação/tipo de acomodação do quarto (Single, Double, Suite, etc.).
    /// </summary>
    public RoomType RoomType { get; set; } = RoomType.Single;

    /// <summary>
    /// Capacidade máxima de ocupantes suportada pelo quarto.
    /// </summary>
    public short Capacity { get; set; }

    /// <summary>
    /// Descrição detalhada dos diferenciais, amenidades e características do quarto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Nome ou número identificador do quarto.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Estado operacional atual do quarto (Disponível, Ocupado, Em Manutenção, etc.).
    /// </summary>
    public RoomStatus Status { get; set; } = RoomStatus.Available;

    /// <summary>
    /// Quantidade mínima de noites exigida para reserva deste quarto.
    /// </summary>
    public int MinimumNights { get; set; }

    /// <summary>
    /// Coleção de registros de disponibilidade e preços configurados para o quarto.
    /// </summary>
    public RoomAvailabilityDto[] Availabilities { get; set; } = [];
}