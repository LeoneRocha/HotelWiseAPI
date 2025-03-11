using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context
{
    public class HotelWiseDbContextMysql : DbContext
    {
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public HotelWiseDbContextMysql(DbContextOptions<HotelWiseDbContextMysql> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure FLUENT API 
            ConfigurationEntitiesHelper.AddConfigurationEntitiesManually(modelBuilder); ConfigurationEntitiesHelper.AddConfigurationEntities(modelBuilder);
        }
    }
}
