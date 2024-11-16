﻿// <auto-generated />
using System;
using HotelWise.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    [DbContext(typeof(HotelWiseDbContextMysql))]
    partial class HotelWiseDbContextMysqlModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HotelWise.Domain.Model.Hotel", b =>
                {
                    b.Property<long>("HotelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("HotelId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("HotelName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("InitialRoomPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<byte>("Stars")
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("StateCode")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("HotelId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            HotelId = 1L,
                            City = "Manaus",
                            Description = "An example hotel",
                            HotelName = "Hotel Example",
                            InitialRoomPrice = 635.576826273363100m,
                            Location = "Rodovia 702 Xavier Travessa",
                            Stars = (byte)1,
                            StateCode = "SP",
                            Tags = "Luxury,Spa",
                            ZipCode = "26237-630"
                        });
                });

            modelBuilder.Entity("HotelWise.Domain.Model.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Email")
                        .HasColumnOrder(3);

                    b.Property<bool>("Enable")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("Enable")
                        .HasColumnOrder(1);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime>("LastAccessDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("LastAccessDate");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ModifyDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Name")
                        .HasColumnOrder(2);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Refresh_token_expiry_time");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Admin = true,
                            CreatedDate = new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1204),
                            Email = "admin@sistemas.com",
                            Enable = true,
                            Language = "pt-BR",
                            LastAccessDate = new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207),
                            Login = "admin",
                            ModifyDate = new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207),
                            Name = "User MOCK ",
                            PasswordHash = new byte[] { 187, 83, 45, 107, 158, 13, 62, 226, 68, 182, 45, 0, 201, 112, 98, 94, 45, 228, 139, 155, 28, 157, 176, 65, 30, 1, 229, 151, 203, 209, 4, 9, 20, 229, 129, 253, 28, 201, 80, 30, 237, 142, 137, 239, 168, 242, 228, 0, 159, 111, 63, 128, 62, 74, 77, 218, 64, 163, 118, 239, 215, 154, 73, 217 },
                            PasswordSalt = new byte[] { 155, 141, 190, 210, 100, 225, 47, 238, 222, 121, 89, 19, 132, 244, 147, 250, 128, 75, 239, 154, 142, 62, 79, 121, 254, 115, 171, 164, 120, 216, 29, 216, 142, 16, 143, 181, 76, 201, 250, 87, 56, 104, 140, 27, 115, 127, 229, 227, 229, 111, 234, 209, 232, 61, 93, 242, 217, 87, 41, 91, 91, 8, 197, 152, 118, 70, 58, 2, 2, 133, 115, 223, 238, 129, 136, 240, 91, 6, 225, 227, 152, 177, 8, 21, 94, 13, 75, 235, 104, 221, 241, 209, 203, 92, 29, 132, 54, 215, 131, 232, 185, 16, 94, 151, 59, 226, 38, 63, 211, 100, 173, 184, 47, 13, 208, 165, 15, 175, 136, 209, 180, 47, 118, 1, 155, 246, 196, 27 },
                            RefreshToken = "",
                            Role = "Admin",
                            TimeZone = "E. South America Standard Time"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
