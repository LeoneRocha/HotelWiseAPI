namespace HotelWise.Domain.Interfaces.Base
{
    public interface IEntityBase
    {
        long Id { get; set; }
        bool Enable { get; set; }
    }
}
