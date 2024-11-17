using HotelWise.Data.Context.Configure;
using HotelWise.Data.Context.Configure.Mock;
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
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.HotelId);
                entity.Property(e => e.HotelName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

                entity.Property(e => e.Description).HasMaxLength(1000).HasColumnType("varchar(1000)");

                entity.Property(e => e.Tags)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)")
                .IsRequired()
                .HasConversion(v => string.Join(',', v), v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

                entity.Property(e => e.Stars)
                .HasConversion<byte>()
                .IsRequired();

                entity.Property(e => e.InitialRoomPrice).IsRequired();
                entity.Property(e => e.ZipCode).HasMaxLength(10).HasColumnType("varchar(10)");
                entity.Property(e => e.StateCode).HasMaxLength(2).HasColumnType("varchar(2)");
                entity.Property(e => e.Location).HasMaxLength(200).HasColumnType("varchar(200)");
                entity.Property(e => e.City).HasMaxLength(200).HasColumnType("varchar(200)");

                entity.HasData(HotelsMockData.GetHotels());
            });

            //Configure FLUENT API 
            ConfigurationEntitiesHelper.AddConfigurationEntities(modelBuilder );
        }
    }
}
