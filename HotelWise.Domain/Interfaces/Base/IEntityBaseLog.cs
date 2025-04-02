namespace HotelWise.Domain.Interfaces.Base
{
    public interface IEntityBaseLog
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifyDate { get; set; }
        DateTime LastAccessDate { get; set; }
    }
}