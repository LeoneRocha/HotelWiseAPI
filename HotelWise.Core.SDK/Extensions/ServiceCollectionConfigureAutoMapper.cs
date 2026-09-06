#if NET8_0_OR_GREATER
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="IServiceCollection"/> para configuração de AutoMapper Profiles.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper em SmartCoreHub.Core.SDK.")]
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
