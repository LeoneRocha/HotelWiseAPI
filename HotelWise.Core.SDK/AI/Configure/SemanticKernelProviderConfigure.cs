#if NET8_0_OR_GREATER
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchConfigure = SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Configuração Semantic Kernel — delega integralmente ao SCH.
/// </summary>
public static class SemanticKernelProviderConfigure
{
    /// <summary>
    /// Configura Semantic Kernel, Qdrant e serviços de IA no <see cref="IServiceCollection"/>.
    /// </summary>
    public static void SetupSemanticKernelProvider<TVector>(IServiceCollection services, IConfiguration configuration)
        where TVector : class =>
        SchConfigure.SetupSemanticKernelProvider<TVector>(services, configuration);
}
#endif
