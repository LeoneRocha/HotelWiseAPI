using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context
{
    public class HotelWiseDbContextMysql : DbContext
    {
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<ChatSessionHistory> ChatSessionHistories { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomAvailability> RoomAvailabilities { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        public HotelWiseDbContextMysql(DbContextOptions<HotelWiseDbContextMysql> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure FLUENT API 
            ConfigurationEntitiesHelper.AddConfigurationEntitiesManually(modelBuilder); 
            ConfigurationEntitiesHelper.AddConfigurationEntities(modelBuilder);
        }
    }
}
