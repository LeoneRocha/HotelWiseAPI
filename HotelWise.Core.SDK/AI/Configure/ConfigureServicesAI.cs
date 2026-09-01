#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using SchConfigure = SmartCoreHub.Core.SDK.Service.AI.Configure.ConfigureServicesAI;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Registro DI genérico de IA — delega integralmente ao SCH.
/// </summary>
public static class ConfigureServicesAI
{
    /// <summary>
    /// Registra fábricas e serviços genéricos de IA via SmartCoreHub.Core.SDK.
    /// </summary>
    /// <param name="services">Coleção de serviços de injeção de dependência.</param>
    public static void RegisterGenericAiServices(IServiceCollection services) =>
        SchConfigure.RegisterGenericAiServices(services);
}
#endif
