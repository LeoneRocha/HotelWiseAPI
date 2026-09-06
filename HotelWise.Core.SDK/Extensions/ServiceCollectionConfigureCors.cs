#if NET8_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="IServiceCollection"/> para configuração CORS genérica
/// (política padrão e política nomeada AllowAnyOrigin), adequada a APIs de desenvolvimento
/// e hosts que expõem Content-Disposition.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors em SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureCors
{
    /// <summary>
    /// Configura as políticas de CORS no container de injeção de dependência.
    /// </summary>
    /// <param name="services">Coleção de serviços do DI.</param>
    public static void Configure(IServiceCollection services) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors.Configure(services);
}
#endif
