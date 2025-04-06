using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class ReservationDto
    {
        public long Id { get; set; } // ID único da reserva
        public long RoomId { get; set; } // ID do quarto associado à reserva
        public DateTime CheckInDate { get; set; } // Data de entrada
        public DateTime CheckOutDate { get; set; } // Data de saída
        public DateTime ReservationDate { get; set; } // Data em que a reserva foi feita
        public decimal TotalAmount { get; set; } // Valor total da reserva
        public string Currency { get; set; } = string.Empty; // Moeda utilizada na reserva (ex.: USD, BRL)
        public ReservationStatus Status { get; set; } =  ReservationStatus.Pending; // Status da reserva (ex.: Confirmed, Cancelled)
        public RoomDto? RoomDetails { get; set; } // Detalhes do quarto associado
    }
}