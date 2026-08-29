using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Dto.Base
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.EntityDtoBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public abstract class EntityDtoBase : HotelWise.Core.SDK.Common.EntityDtoBase, IEntityDto
    {
    }
}
