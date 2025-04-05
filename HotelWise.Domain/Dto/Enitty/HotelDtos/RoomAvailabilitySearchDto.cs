namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class RoomAvailabilitySearchDto
    {
        public long HotelId { get; set; } // ID do hotel
        public DateTime StartDate { get; set; } // Data inicial obrigatória
        public DateTime? EndDate { get; set; } // Data final opcional
    }

}
