#if NET8_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="IServiceCollection"/> para configuração CORS genérica
/// (política padrão e política nomeada AllowAnyOrigin), adequada a APIs de desenvolvimento
/// e hosts que expõem Content-Disposition.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureCors
{
    public static void Configure(IServiceCollection services) =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors.Configure(services);
}

#endif
