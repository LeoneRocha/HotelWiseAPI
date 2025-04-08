using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class RoomDto
    {
        public long Id { get; set; } // ID único do quarto
        public long HotelId { get; set; } // ID do hotel ao qual o quarto pertence
        public RoomType RoomType { get; set; } = RoomType.Single; // Tipo do quarto (Ex.: Standard, Deluxe, Suite)
        public short Capacity { get; set; } // Capacidade máxima do quarto (número de pessoas)
        public string Description { get; set; } = string.Empty; // Descrição detalhada do quarto
         
        public RoomStatus Status { get; set; } = RoomStatus.Available; // Status do quarto (ex.: Available, Unavailable)
        public int MinimumNights { get; set; } // Mínimo de noites exigido para reserva
        public RoomAvailabilityDto[] Availabilities { get; set; } = []; // Lista de disponibilidades do quarto
    }
} 