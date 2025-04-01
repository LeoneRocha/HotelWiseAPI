using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Model.HotelModels
{
    public class Room : IEntityBaseLog
    {
        public long Id { get; set; } // Chave primária
        public long HotelId { get; set; } // Relacionamento com Hotel 
        public RoomType RoomType { get; set; } // Enum para tipo de quarto
        public short Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public RoomStatus Status { get; set; } // Enum para status

        public short MinimumNights { get; set; } = 1;

        public Hotel? Hotel { get; set; } // Relacionamento com Hotel

        #region MyRegion
        public User? CreatedUser { get; set; }
        public long? CreatedUserId { get; set; }
        public User? ModifyUser { get; set; }
        public long? ModifyUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        #endregion
    }
}