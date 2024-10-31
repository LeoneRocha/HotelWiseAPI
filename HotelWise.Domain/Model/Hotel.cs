namespace HotelWise.Domain.Model
{
    public class Hotel
    {
        public ulong HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[] Tags { get; set; } = [];
        public int Stars { get; set; }
        public decimal InitialRoomPrice { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

}
