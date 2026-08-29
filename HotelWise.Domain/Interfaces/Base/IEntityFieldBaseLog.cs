using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Base
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — campos escalares canônicos no Core;
    /// navegações <see cref="User"/> permanecem no host (domínio).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.IEntityFieldBaseLog.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DOMAIN")]
    public interface IEntityFieldBaseLog : HotelWise.Core.SDK.Abstractions.IEntityFieldBaseLog
    {
        User? CreatedUser { get; set; }
        User? ModifyUser { get; set; }
    }
}
