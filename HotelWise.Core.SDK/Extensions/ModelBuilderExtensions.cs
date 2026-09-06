#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="ModelBuilder"/> para descoberta e aplicação automática
/// de classes <see cref="IEntityTypeConfiguration{TEntity}"/> em um assembly,
/// excluindo tipos já configurados manualmente.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions em SmartCoreHub.Core.SDK.")]
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Aplica as configurações de entidades encontradas no assembly informado ao <see cref="ModelBuilder"/>,
    /// ignorando os tipos já configurados manualmente.
    /// </summary>
    /// <param name="modelBuilder">Model builder do EF Core.</param>
    /// <param name="assembly">Assembly a ser escaneado.</param>
    /// <param name="manuallyConfiguredTypes">Lista de tipos já configurados manualmente.</param>
    public static void AddConfigurationEntities(this ModelBuilder modelBuilder, Assembly assembly, List<Type> manuallyConfiguredTypes) =>
        SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions.AddConfigurationEntities(modelBuilder, assembly, manuallyConfiguredTypes);
}
#endif
