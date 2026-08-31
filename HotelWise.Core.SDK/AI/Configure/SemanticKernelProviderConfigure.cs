#if NET8_0_OR_GREATER
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchConfigure = SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Configuração Semantic Kernel — delega integralmente ao SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
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
