using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context.Configure.Helper
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Extensions.ModelBuilderExtensions.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_REPO")]
    public static class ModelBuilderExtensions
    {
        public static void AddConfigurationEntities(this ModelBuilder modelBuilder, Assembly assembly, List<Type> manuallyConfiguredTypes) =>
            HotelWise.Core.SDK.Extensions.ModelBuilderExtensions.AddConfigurationEntities(modelBuilder, assembly, manuallyConfiguredTypes);
    }
}
