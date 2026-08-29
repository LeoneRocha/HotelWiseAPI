using HotelWise.Data.Context.Configure.Entity;
using HotelWise.Data.Context.Configure.Entity.HotelModelConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelWise.Data.Context.Configure.Helper
{
    /// <summary>
    /// ⚠️ Helper hotel-specific — permanece no host (HotelConfiguration/UserConfiguration).
    /// Parte genérica: HotelWise.Core.SDK.Extensions.ModelBuilderExtensions.
    /// </summary>
    [Obsolete(
        "Wiring hotel-specific permanece no host. Use HotelWise.Core.SDK.Extensions.ModelBuilderExtensions para discovery genérico.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_REPO")]
    public static class ConfigurationEntitiesHelper
    {
        public static void AddConfigurationEntitiesManually(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public static void AddConfigurationEntities(ModelBuilder modelBuilder)
        {
            List<Type> manuallyConfiguredTypes = new List<Type>
            {
                typeof(HotelConfiguration),
                typeof(UserConfiguration)
            };
            modelBuilder.AddConfigurationEntities(Assembly.GetExecutingAssembly(), manuallyConfiguredTypes);
        }
    }
}
