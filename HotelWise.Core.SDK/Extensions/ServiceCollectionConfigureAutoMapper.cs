#if NET8_0_OR_GREATER
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="IServiceCollection"/> para registro genérico de
/// profiles AutoMapper tipados no container de DI.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureAutoMapper
{
    /// <summary>
    /// Registra um profile AutoMapper concreto no serviço de mapeamento.
    /// </summary>
    /// <typeparam name="TProfile">Tipo do profile (<see cref="Profile"/>) a adicionar.</typeparam>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void AddProfile<TProfile>(IServiceCollection services) where TProfile : Profile, new()
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<TProfile>());
    }
}
#endif
