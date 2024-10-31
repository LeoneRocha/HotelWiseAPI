using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Context
{
    public class HotelWiseDbContextMysql  :  DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }

        public HotelWiseDbContextMysql(DbContextOptions<HotelWiseDbContextMysql> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.HotelId);
                entity.Property(e => e.HotelName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Tags).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
                entity.Property(e => e.Stars).IsRequired();
                entity.Property(e => e.InitialRoomPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.ZipCode).HasMaxLength(10);
                entity.Property(e => e.Location).HasMaxLength(200);

                entity.HasData(new Hotel
                {
                    HotelId = 1,
                    HotelName = "Hotel Example",
                    Description = "An example hotel",
                    Tags = new[] { "Luxury", "Spa" },
                    Stars = 5,
                    InitialRoomPrice = 200.00m,
                    ZipCode = "12345",
                    Location = "Example City"
                });
            });
        }
    }

}
