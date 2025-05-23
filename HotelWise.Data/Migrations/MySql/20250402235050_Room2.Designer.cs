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
    [Migration("20250402235050_Room2")]
    partial class Room2
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
                            City = "Ribeirão Preto",
                            CreatedDate = new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(8544),
                            CreatedUserId = 1L,
                            Description = "An example hotel",
                            HotelName = "Hotel Example",
                            InitialRoomPrice = 404.313868838800900m,
                            Location = "Travessa 1398 Lavínia Avenida",
                            ModifyDate = new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(8545),
                            ModifyUserId = 1L,
                            Stars = (byte)2,
                            StateCode = "TO",
                            Tags = "Luxury,Spa",
                            ZipCode = "05086-749"
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
                            CreatedDate = new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9903),
                            Email = "admin@sistemas.com",
                            Enable = true,
                            Language = "pt-BR",
                            LastAccessDate = new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9903),
                            Login = "admin",
                            ModifyDate = new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9904),
                            Name = "User MOCK ",
                            PasswordHash = new byte[] { 134, 89, 65, 106, 93, 229, 198, 160, 66, 6, 148, 69, 222, 213, 210, 215, 159, 152, 53, 36, 132, 97, 203, 222, 211, 153, 24, 222, 224, 93, 69, 16, 29, 9, 145, 241, 106, 10, 82, 205, 73, 36, 40, 18, 203, 9, 174, 16, 137, 247, 211, 165, 6, 210, 190, 184, 236, 222, 59, 158, 252, 163, 201, 113 },
                            PasswordSalt = new byte[] { 44, 226, 248, 24, 117, 208, 254, 115, 9, 159, 52, 77, 233, 6, 52, 233, 18, 188, 45, 237, 239, 217, 219, 187, 48, 87, 5, 191, 57, 144, 148, 66, 9, 148, 10, 200, 176, 71, 167, 8, 97, 255, 106, 65, 199, 172, 15, 190, 6, 168, 61, 182, 112, 55, 137, 15, 198, 213, 31, 61, 23, 181, 81, 80, 158, 60, 9, 38, 174, 38, 234, 102, 82, 141, 119, 158, 72, 85, 31, 49, 225, 249, 177, 53, 174, 126, 42, 12, 49, 117, 59, 238, 246, 51, 147, 28, 61, 1, 87, 174, 16, 69, 155, 170, 231, 15, 127, 14, 153, 213, 106, 21, 55, 225, 184, 76, 27, 119, 167, 165, 253, 63, 93, 233, 79, 47, 176, 238 },
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
                        .WithMany("RoomAvailabilities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotelWise.Domain.Model.HotelModels.Room", b =>
                {
                    b.Navigation("RoomAvailabilities");
                });
#pragma warning restore 612, 618
        }
    }
}
