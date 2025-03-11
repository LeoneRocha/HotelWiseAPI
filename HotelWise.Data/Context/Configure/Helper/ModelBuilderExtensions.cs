using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelWise.Data.Context.Configure.Helper
{
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
            var listAdd = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)) && !manuallyConfiguredTypes.Contains(t) && t.Name.EndsWith("Configuration")).ToArray();
            return listAdd;
        }
    }
}
