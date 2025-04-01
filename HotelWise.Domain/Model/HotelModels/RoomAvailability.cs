namespace HotelWise.Domain.Model.HotelModels
{
    public class RoomAvailability
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public RoomPriceAndAvailabilityItem[] AvailabilityWithPrice { get; set; } = [];        
        public Room Room { get; set; }

        public DateTime StartDate { get; set; } // Data inicial do período
        public DateTime EndDate { get; set; }

    }
}
