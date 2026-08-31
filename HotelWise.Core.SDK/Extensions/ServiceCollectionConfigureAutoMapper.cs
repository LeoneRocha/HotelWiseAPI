#if NET8_0_OR_GREATER
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ServiceCollectionConfigureAutoMapper
{
    public static void AddProfile<TProfile>(IServiceCollection services)
        where TProfile : Profile, new() =>
        SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper.AddProfile<TProfile>(services);
}

#endif
