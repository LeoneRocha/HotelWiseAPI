using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Model.HotelModels
{
    public class Hotel : IEntityFieldBaseLog
    {
        public long HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[] Tags { get; set; } = [];
        public byte Stars { get; set; }
        public decimal InitialRoomPrice { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;

        public User? CreatedUser { get; set; }
        public long? CreatedUserId { get; set; }
        public User? ModifyUser { get; set; }
        public long? ModifyUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}