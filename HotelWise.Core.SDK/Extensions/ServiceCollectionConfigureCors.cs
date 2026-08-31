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
    /// <summary>
    /// Aplica a configuração CORS padrão do SDK ao container de serviços.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void Configure(IServiceCollection services)
    {
        AddCors(services);
    }

    /// <summary>
    /// Registra as políticas CORS (padrão e AllowAnyOrigin).
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
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
