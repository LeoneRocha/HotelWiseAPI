﻿// <auto-generated />
using System;
using HotelWise.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    [DbContext(typeof(HotelWiseDbContextMysql))]
    [Migration("20241120195647_LogDataAddUpdate")]
    partial class LogDataAddUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("CreatedUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(1L);

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

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("ModifyUserId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(1L);

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

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ModifyUserId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            HotelId = 1L,
                            City = "Santo André",
                            CreatedDate = new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2547),
                            CreatedUserId = 1L,
                            Description = "An example hotel",
                            HotelName = "Hotel Example",
                            InitialRoomPrice = 987.81193357016500m,
                            Location = "Avenida 07287 Martins Avenida",
                            ModifyDate = new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2550),
                            ModifyUserId = 1L,
                            Stars = (byte)2,
                            StateCode = "PI",
                            Tags = "Luxury,Spa",
                            ZipCode = "31481-061"
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
                            CreatedDate = new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4545),
                            Email = "admin@sistemas.com",
                            Enable = true,
                            Language = "pt-BR",
                            LastAccessDate = new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546),
                            Login = "admin",
                            ModifyDate = new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546),
                            Name = "User MOCK ",
                            PasswordHash = new byte[] { 216, 63, 57, 138, 3, 180, 164, 1, 225, 4, 52, 170, 164, 223, 203, 136, 68, 137, 127, 139, 221, 172, 247, 18, 25, 192, 246, 194, 139, 164, 161, 56, 107, 134, 115, 95, 248, 62, 10, 150, 248, 53, 121, 37, 87, 132, 183, 216, 48, 56, 111, 15, 27, 137, 124, 56, 86, 95, 21, 17, 15, 193, 124, 95 },
                            PasswordSalt = new byte[] { 180, 226, 18, 138, 49, 114, 199, 121, 145, 232, 124, 229, 135, 189, 158, 215, 190, 20, 187, 174, 181, 48, 168, 141, 30, 66, 25, 136, 175, 12, 138, 59, 48, 14, 131, 71, 163, 42, 68, 220, 251, 70, 128, 154, 180, 77, 241, 35, 235, 223, 161, 27, 135, 13, 67, 5, 122, 77, 122, 198, 125, 203, 151, 205, 2, 169, 121, 212, 187, 124, 110, 179, 236, 193, 15, 181, 249, 98, 42, 121, 59, 93, 90, 60, 158, 64, 224, 205, 182, 48, 47, 195, 78, 23, 49, 122, 203, 121, 6, 121, 102, 133, 239, 98, 56, 93, 202, 168, 10, 56, 7, 100, 27, 139, 121, 151, 36, 222, 79, 123, 20, 92, 104, 188, 107, 140, 143, 228 },
                            RefreshToken = "",
                            Role = "Admin",
                            TimeZone = "E. South America Standard Time"
                        });
                });

            modelBuilder.Entity("HotelWise.Domain.Model.Hotel", b =>
                {
                    b.HasOne("HotelWise.Domain.Model.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("HotelWise.Domain.Model.User", "ModifyUser")
                        .WithMany()
                        .HasForeignKey("ModifyUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("ModifyUser");
                });
#pragma warning restore 612, 618
        }
    }
}