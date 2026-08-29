#if NET8_0_OR_GREATER
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões de descoberta/aplicação de IEntityTypeConfiguration.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void AddConfigurationEntities(this ModelBuilder modelBuilder, Assembly assembly, List<Type> manuallyConfiguredTypes)
    {
        Type[] configTypes = ListClassConfiguration(assembly, manuallyConfiguredTypes).OrderBy(t => t.Name).ToArray();

        foreach (var configType in configTypes)
        {
            dynamic configInstance = Activator.CreateInstance(configType)!;
            modelBuilder.ApplyConfiguration(configInstance);
        }
    }

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
