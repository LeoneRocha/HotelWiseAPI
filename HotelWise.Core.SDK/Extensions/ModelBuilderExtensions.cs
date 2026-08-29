#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de <see cref="ModelBuilder"/> para descoberta e aplicação automática
/// de classes <see cref="IEntityTypeConfiguration{TEntity}"/> em um assembly,
/// excluindo tipos já configurados manualmente.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Descobre e aplica todas as configurações de entidade do assembly informado.
    /// </summary>
    /// <param name="modelBuilder">Builder do modelo EF Core.</param>
    /// <param name="assembly">Assembly onde procurar classes *Configuration.</param>
    /// <param name="manuallyConfiguredTypes">Tipos já aplicados manualmente (excluídos da descoberta).</param>
    public static void AddConfigurationEntities(this ModelBuilder modelBuilder, Assembly assembly, List<Type> manuallyConfiguredTypes)
    {
        Type[] configTypes = ListClassConfiguration(assembly, manuallyConfiguredTypes).OrderBy(t => t.Name).ToArray();

        foreach (var configType in configTypes)
        {
            dynamic configInstance = Activator.CreateInstance(configType)!;
            modelBuilder.ApplyConfiguration(configInstance);
        }
    }

    /// <summary>
    /// Lista tipos concretos que implementam <see cref="IEntityTypeConfiguration{TEntity}"/>
    /// e cujo nome termina com "Configuration".
    /// </summary>
    /// <param name="assembly">Assembly de origem.</param>
    /// <param name="manuallyConfiguredTypes">Tipos a ignorar.</param>
    /// <returns>Array de tipos de configuração encontrados.</returns>
    private static Type[] ListClassConfiguration(Assembly assembly, List<Type> manuallyConfiguredTypes)
    {
        return assembly.GetTypes()
            .Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                && !manuallyConfiguredTypes.Contains(t)
                && t.Name.EndsWith("Configuration"))
            .ToArray();
    }
}
#endif
