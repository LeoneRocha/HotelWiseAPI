namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class RoomAvailabilitySearchDto
    {
        public required long HotelId { get; set; } // ID do hotel
        public required DateTime StartDate { get; set; } // Data inicial obrigatória
        public DateTime? EndDate { get; set; } // Data final opcional
        public required string Currency { get; set; } = "USD";
    } 
}
