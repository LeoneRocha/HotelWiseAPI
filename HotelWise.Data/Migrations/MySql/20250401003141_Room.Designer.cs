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
    [Migration("20250401003141_Room")]
    partial class Room
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HotelWise.Domain.Model.AI.ChatSessionHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CountMessages")
                        .HasColumnType("int");

                    b.Property<string>("IdToken")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<long?>("IdUser")
                        .HasColumnType("bigint");

                    b.Property<string>("PromptMessageHistory")
                        .IsRequired()
                        .HasMaxLength(65535)
                        .HasColumnType("text");

                    b.Property<DateTime>("SessionDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("TotalTokensMessage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdToken")
                        .HasDatabaseName("IX_ChatSessionHistory_IdToken");

                    b.HasIndex("SessionDateTime")
                        .HasDatabaseName("IX_ChatSessionHistory_SessionDateTime");

                    b.ToTable("ChatSessionHistory", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Hotel", b =>
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
                            City = "São Vicente",
                            CreatedDate = new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(3228),
                            CreatedUserId = 1L,
                            Description = "An example hotel",
                            HotelName = "Hotel Example",
                            InitialRoomPrice = 623.225574471303100m,
                            Location = "Avenida 37023 Costa Rodovia",
                            ModifyDate = new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(3230),
                            ModifyUserId = 1L,
                            Stars = (byte)1,
                            StateCode = "CE",
                            Tags = "Luxury,Spa",
                            ZipCode = "75702-587"
                        });
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint unsigned");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CheckInDate")
                        .HasDatabaseName("IX_Reservation_CheckInDate");

                    b.HasIndex("CheckOutDate")
                        .HasDatabaseName("IX_Reservation_CheckOutDate");

                    b.HasIndex("ReservationDate")
                        .HasDatabaseName("IX_Reservation_ReservationDate");

                    b.HasIndex("RoomId")
                        .HasDatabaseName("IX_Reservation_RoomId");

                    b.ToTable("Reservation", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Room", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<short>("Capacity")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("CreatedUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<long>("HotelId")
                        .HasColumnType("bigint");

                    b.Property<short>("MinimumNights")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)1);

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("ModifyUserId")
                        .HasColumnType("bigint");

                    b.Property<byte>("RoomType")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("HotelId")
                        .HasDatabaseName("IX_Room_HotelId");

                    b.HasIndex("ModifyUserId");

                    b.HasIndex("RoomType")
                        .HasDatabaseName("IX_Room_RoomType");

                    b.ToTable("Room", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.RoomAvailability", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AvailabilityWithPrice")
                        .IsRequired()
                        .HasMaxLength(65535)
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("EndDate")
                        .HasDatabaseName("IX_RoomAvailability_EndDate");

                    b.HasIndex("RoomId")
                        .HasDatabaseName("IX_RoomAvailability_RoomId");

                    b.HasIndex("StartDate")
                        .HasDatabaseName("IX_RoomAvailability_StartDate");

                    b.ToTable("RoomAvailability", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
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
                            CreatedDate = new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4642),
                            Email = "admin@sistemas.com",
                            Enable = true,
                            Language = "pt-BR",
                            LastAccessDate = new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4643),
                            Login = "admin",
                            ModifyDate = new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4643),
                            Name = "User MOCK ",
                            PasswordHash = new byte[] { 135, 140, 110, 13, 35, 170, 175, 196, 23, 165, 235, 53, 206, 189, 241, 77, 110, 220, 103, 48, 44, 131, 24, 167, 42, 56, 110, 103, 214, 95, 158, 156, 254, 33, 109, 21, 253, 12, 186, 30, 236, 140, 78, 170, 85, 134, 188, 181, 211, 33, 204, 233, 232, 85, 163, 107, 135, 79, 189, 241, 14, 99, 75, 198 },
                            PasswordSalt = new byte[] { 57, 41, 246, 85, 73, 51, 206, 77, 116, 78, 111, 63, 103, 97, 211, 170, 140, 171, 28, 59, 203, 158, 9, 233, 106, 167, 85, 198, 213, 249, 135, 197, 11, 64, 167, 137, 105, 156, 172, 63, 103, 235, 221, 23, 94, 229, 156, 41, 23, 157, 42, 195, 32, 179, 245, 171, 4, 188, 45, 41, 134, 0, 160, 4, 233, 189, 165, 4, 229, 214, 179, 65, 81, 210, 87, 89, 242, 208, 28, 210, 117, 94, 120, 148, 28, 75, 239, 237, 237, 167, 141, 184, 67, 32, 180, 77, 223, 30, 61, 68, 86, 219, 2, 54, 54, 182, 27, 22, 244, 152, 87, 177, 2, 187, 245, 19, 153, 207, 8, 107, 3, 109, 153, 244, 66, 191, 221, 69 },
                            RefreshToken = "",
                            Role = "Admin",
                            TimeZone = "E. South America Standard Time"
                        });
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Hotel", b =>
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

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Reservation", b =>
                {
                    b.HasOne("HotelWise.Domain.Model.HotelModels.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Room", b =>
                {
                    b.HasOne("HotelWise.Domain.Model.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("HotelWise.Domain.Model.HotelModels.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotelWise.Domain.Model.User", "ModifyUser")
                        .WithMany()
                        .HasForeignKey("ModifyUserId");

                    b.Navigation("CreatedUser");

                    b.Navigation("Hotel");

                    b.Navigation("ModifyUser");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.RoomAvailability", b =>
                {
                    b.HasOne("HotelWise.Domain.Model.HotelModels.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });
#pragma warning restore 612, 618
        }
    }
}
