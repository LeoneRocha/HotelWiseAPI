namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IEntityBaseLog
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifyDate { get; set; }
        DateTime LastAccessDate { get; set; }
    }
}