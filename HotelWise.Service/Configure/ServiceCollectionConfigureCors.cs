using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureCors.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DI")]
    public static class ServiceCollectionConfigureCors
    {
        public static void Configure(IServiceCollection services) =>
            HotelWise.Core.SDK.Extensions.ServiceCollectionConfigureCors.Configure(services);
    }
}
