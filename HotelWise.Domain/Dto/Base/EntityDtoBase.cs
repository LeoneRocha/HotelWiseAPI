using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Dto.Base
{
    public abstract class EntityDtoBase : IEntityDto
    {
        public long Id { get; set; }
        public bool Enable { get; set; }
    }
}
