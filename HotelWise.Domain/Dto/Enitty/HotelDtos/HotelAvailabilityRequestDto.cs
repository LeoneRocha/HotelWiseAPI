namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class HotelAvailabilityRequestDto
    {
        public required long HotelId { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string Currency { get; set; } = "USD";
    } 
}
