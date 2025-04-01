using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Entity
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservation");
            HelperCharSet.AddCharSet(builder);

            #region KEY
            // Definição de chave primária 
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            #endregion  KEY
             
            builder.Property(r => r.CheckInDate)
                   .IsRequired();
            builder.Property(r => r.CheckOutDate)
                   .IsRequired();

            builder.Property(r => r.ReservationDate)
                   .IsRequired();
            builder.Property(r => r.TotalAmount)
                   .IsRequired();

            builder.Property(r => r.Currency)
                   .IsRequired()
                   .HasMaxLength(3)
                   .HasColumnType("varchar(3)");

            builder.Property(r => r.Status)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.HasOne(r => r.Room)
                   .WithMany()
                   .HasForeignKey(r => r.RoomId);


            // Adicionando índices
            builder.HasIndex(r => r.CheckInDate).HasDatabaseName("IX_Reservation_CheckInDate");
            builder.HasIndex(r => r.CheckOutDate).HasDatabaseName("IX_Reservation_CheckOutDate");
            builder.HasIndex(r => r.RoomId).HasDatabaseName("IX_Reservation_RoomId");
            builder.HasIndex(r => r.ReservationDate).HasDatabaseName("IX_Reservation_ReservationDate");


        }
    }

}
