using HotelWise.Data.Context.Configure.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context.Configure
{
    public static class ConfigurationEntitiesHelper
    {
        public static void AddConfigurationEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}