using AutoMapper;

namespace HotelWise.Service.Generic
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Services.GenericVectorStoreServiceBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public abstract class GenericVectorStoreServiceBase : HotelWise.Core.SDK.AI.Services.GenericVectorStoreServiceBase
    {
        protected GenericVectorStoreServiceBase(IMapper mapper, Serilog.ILogger logger)
            : base(mapper, logger)
        {
        }
    }
}
