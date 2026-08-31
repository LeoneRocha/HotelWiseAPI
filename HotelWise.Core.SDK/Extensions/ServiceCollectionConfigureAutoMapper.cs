#if NET8_0_OR_GREATER
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="IServiceCollection"/> para configuração de AutoMapper Profiles.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureAutoMapper
{
    /// <summary>
    /// Registra um profile AutoMapper no container de injeção de dependência.
    /// </summary>
    /// <typeparam name="TProfile">Tipo do Profile do AutoMapper.</typeparam>
    /// <param name="services">Coleção de serviços do DI.</param>
    public static void AddProfile<TProfile>(IServiceCollection services)
        where TProfile : Profile, new() =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper.AddProfile<TProfile>(services);
}
#endif
