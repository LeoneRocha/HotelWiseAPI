#if NET8_0_OR_GREATER
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchConfigure = SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Configuração Semantic Kernel — delega integralmente ao SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure em SmartCoreHub.Core.SDK.")]
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
