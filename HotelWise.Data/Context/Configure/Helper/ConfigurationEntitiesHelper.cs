using System.Reflection;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Data.Context.Configure.Entity;
using HotelWise.Data.Context.Configure.Entity.HotelModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context.Configure.Helper;

/// <summary>
/// Classe auxiliar para registro manual e por reflexão das configurações de mapeamento de entidades EF Core.
/// </summary>
public static class ConfigurationEntitiesHelper
{
    /// <summary>
    /// Registra manualmente as configurações prioritárias de entidades (<see cref="HotelConfiguration"/> e <see cref="UserConfiguration"/>).
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelos do EF Core.</param>
    public static void AddConfigurationEntitiesManually(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    /// <summary>
    /// Descobre e registra automaticamente todas as classes de configuração do assembly atual via reflexão.
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelos do EF Core.</param>
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
