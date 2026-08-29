#if NET8_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Configuração CORS genérica.
/// </summary>
public static class ServiceCollectionConfigureCors
{
    public static void Configure(IServiceCollection services)
    {
        AddCors(services);
    }

    private static void AddCors(IServiceCollection services)
    {
#pragma warning disable S5122
        services.AddCors(options => options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition");
        }));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
#pragma warning restore S5122
    }
}
#endif
