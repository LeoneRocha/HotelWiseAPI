#if NET8_0_OR_GREATER
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Registro genérico de AutoMapper por profile tipado.
/// </summary>
public static class ServiceCollectionConfigureAutoMapper
{
    public static void AddProfile<TProfile>(IServiceCollection services) where TProfile : Profile, new()
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<TProfile>());
    }
}
#endif
