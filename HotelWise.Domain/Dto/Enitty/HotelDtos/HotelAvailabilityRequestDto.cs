namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class HotelAvailabilityRequestDto
    {
        public long HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
