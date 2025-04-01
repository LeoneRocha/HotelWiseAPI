using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Model.HotelModels
{
    public class Reservation
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public long? UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public ReservationStatus Status { get; set; }

        public Room Room { get; set; }
    }

}
