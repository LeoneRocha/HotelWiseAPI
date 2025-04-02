namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class RoomDto
    {
        public long Id { get; set; } // ID único do quarto
        public long HotelId { get; set; } // ID do hotel ao qual o quarto pertence
        public string RoomType { get; set; } = string.Empty; // Tipo do quarto (Ex.: Standard, Deluxe, Suite)
        public int Capacity { get; set; } // Capacidade máxima do quarto (número de pessoas)
        public string Description { get; set; } = string.Empty; // Descrição detalhada do quarto
        public decimal PricePerNight { get; set; } // Preço por noite
        public string Currency { get; set; } = "USD"; // Moeda do preço (ex.: USD, BRL)
        public string Status { get; set; } = string.Empty; // Status do quarto (ex.: Available, Unavailable)
        public int MinimumNights { get; set; } // Mínimo de noites exigido para reserva
        public RoomAvailabilityDto[] Availabilities { get; set; } // Lista de disponibilidades do quarto
    }
}