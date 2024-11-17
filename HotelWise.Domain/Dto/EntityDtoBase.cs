using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Dto
{
    public abstract class EntityDtoBase : IEntityDto
    {
        public long Id { get; set; }
        public bool Enable { get; set; }
    }
}
