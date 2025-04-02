using HotelWise.Data.Context.Configure.Mock;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Entity
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(e => e.HotelId);
            builder.Property(e => e.HotelName).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

            builder.Property(e => e.Description).HasMaxLength(1000).HasColumnType("varchar(1000)");

            builder.Property(e => e.Tags)
            .HasMaxLength(500)
            .HasColumnType("varchar(500)")
            .IsRequired()
            .HasConversion(v => string.Join(',', v), v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            builder.Property(e => e.Stars)
            .HasConversion<byte>()
            .IsRequired();

            builder.Property(e => e.InitialRoomPrice).IsRequired();
            builder.Property(e => e.ZipCode).HasMaxLength(10).HasColumnType("varchar(10)");
            builder.Property(e => e.StateCode).HasMaxLength(2).HasColumnType("varchar(2)");
            builder.Property(e => e.Location).HasMaxLength(200).HasColumnType("varchar(200)");
            builder.Property(e => e.City).HasMaxLength(200).HasColumnType("varchar(200)");

            builder.Property(e => e.CreatedUserId).IsRequired(false).HasDefaultValue((long)1);
            builder.Property(e => e.ModifyUserId).IsRequired(false).HasDefaultValue((long)1);

            builder.Property(e => e.CreatedDate).IsRequired(true);
            builder.Property(e => e.ModifyUserId).IsRequired(true);

            // Relationship                                    
            builder.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedUserId);
            builder.HasOne(e => e.ModifyUser).WithMany().HasForeignKey(e => e.ModifyUserId);

            //DATA LOAD
            builder.HasData(HotelsMockData.GetHotels());
        }
    }
}