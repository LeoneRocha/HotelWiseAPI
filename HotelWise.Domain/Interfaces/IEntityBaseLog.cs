using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IEntityBaseLog
    {
        User? CreatedUser { get; set; }
        User? ModifyUser { get; set; }
        long? CreatedUserId { get; set; }
        long? ModifyUserId { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime ModifyDate { get; set; }
    }
}