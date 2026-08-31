#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="ModelBuilder"/> para descoberta e aplicação automática
/// de classes <see cref="IEntityTypeConfiguration{TEntity}"/> em um assembly,
/// excluindo tipos já configurados manualmente.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ModelBuilderExtensions
{
    public static void AddConfigurationEntities(this ModelBuilder modelBuilder, Assembly assembly, List<Type> manuallyConfiguredTypes) =>
        SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions.AddConfigurationEntities(modelBuilder, assembly, manuallyConfiguredTypes);
}

#endif
