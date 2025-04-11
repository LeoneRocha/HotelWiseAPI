using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace HotelWise.Data.Context.Configure.Entity.HotelModelConfigurations
{
    public class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
    {
        public void Configure(EntityTypeBuilder<RoomAvailability> builder)
        {

            builder.ToTable("RoomAvailability");
            HelperCharSet.AddCharSet(builder);

            #region KEY
            // Definição de chave primária 
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            #endregion  KEY

            builder.Property(ra => ra.StartDate)
                       .IsRequired();

            builder.Property(ra => ra.EndDate)
                .IsRequired();

            builder.Property(ra => ra.Currency)
                         .HasMaxLength(3)
                   .IsRequired();

            builder.Property(ra => ra.AvailabilityWithPrice)
                   .IsRequired()
                   .HasMaxLength(EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase(ETypeDataBase.Mysql))
                   .HasColumnType(EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase(ETypeDataBase.Mysql))
                   .HasConversion(
                       v => JsonConvert.SerializeObject(v),
                       v => JsonConvert.DeserializeObject<RoomPriceAndAvailabilityItem[]>(v)!);

            builder.HasOne(ra => ra.Room)
                   .WithMany()
                   .HasForeignKey(ra => ra.RoomId);

            // Adicionando índices para otimização
            builder.HasIndex(ra => ra.StartDate).HasDatabaseName("IX_RoomAvailability_StartDate");
            builder.HasIndex(ra => ra.EndDate).HasDatabaseName("IX_RoomAvailability_EndDate");
            builder.HasIndex(ra => ra.RoomId).HasDatabaseName("IX_RoomAvailability_RoomId");
        }
    } 
}
