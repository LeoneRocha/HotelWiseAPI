namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IEntityBase
    {
        long Id { get; set; }
        bool Enable { get; set; }
    }
}
