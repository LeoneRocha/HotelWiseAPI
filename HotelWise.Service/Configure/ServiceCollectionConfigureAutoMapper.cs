using HotelWise.Domain.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    /// <summary>
    /// ⚠️ Profile hotel permanece no Domain; Core expõe AddProfile&lt;TProfile&gt;.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAutoMapper.AddProfile&lt;T&gt;.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DI")]
    public static class ServiceCollectionConfigureAutoMapper
    {
        public static void Configure(IServiceCollection services)
        {
            HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureAutoMapper.AddProfile<AutoMapperProfile>(services);
        }
    }
}
