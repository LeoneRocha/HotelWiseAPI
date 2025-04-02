﻿using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Entity
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room");
            HelperCharSet.AddCharSet(builder);

            #region KEY
            // Definição de chave primária 
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            #endregion  KEY

            builder.Property(r => r.RoomType)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.Property(r => r.Capacity)
                   .IsRequired();

            builder.Property(r => r.Description)
                   .HasMaxLength(1000)
                   .HasColumnType("varchar(1000)");

            builder.Property(r => r.Status)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.Property(r => r.MinimumNights)
                .IsRequired()
                .HasDefaultValue(1); // Valor padrão: 1 noite mínim


            builder.HasOne(r => r.Hotel)
                   .WithMany()
                   .HasForeignKey(r => r.HotelId);


            // Adicionando índices
            builder.HasIndex(r => r.HotelId).HasDatabaseName("IX_Room_HotelId");
            builder.HasIndex(r => r.RoomType).HasDatabaseName("IX_Room_RoomType");
        }
    }

}
