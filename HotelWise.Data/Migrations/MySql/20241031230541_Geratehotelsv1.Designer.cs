﻿// <auto-generated />
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
    [Migration("20241031230541_Geratehotelsv1")]
    partial class Geratehotelsv1
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

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

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

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
                            Description = "An example hotel",
                            HotelName = "Hotel Example",
                            InitialRoomPrice = 200.00m,
                            Location = "Example City",
                            Stars = (byte)5,
                            Tags = "Luxury,Spa",
                            ZipCode = "12345"
                        },
                        new
                        {
                            HotelId = 2L,
                            Description = "Est aliquid distinctio aut debitis quibusdam ratione.",
                            HotelName = "Xavier - Barros",
                            InitialRoomPrice = 326.553639874863400m,
                            Location = "Santa Maria",
                            Stars = (byte)1,
                            Tags = "id,natus,ratione",
                            ZipCode = "06785-646"
                        },
                        new
                        {
                            HotelId = 3L,
                            Description = "Fugit libero ex et est officia.",
                            HotelName = "Albuquerque, Moreira and Barros",
                            InitialRoomPrice = 107.37490893956946100m,
                            Location = "Santo André",
                            Stars = (byte)4,
                            Tags = "ab,omnis,enim",
                            ZipCode = "56242-460"
                        },
                        new
                        {
                            HotelId = 4L,
                            Description = "Vitae sint voluptatem blanditiis et porro a aut.",
                            HotelName = "Franco LTDA",
                            InitialRoomPrice = 231.588574865117200m,
                            Location = "Porto Velho",
                            Stars = (byte)4,
                            Tags = "esse,neque,est",
                            ZipCode = "11972-342"
                        },
                        new
                        {
                            HotelId = 5L,
                            Description = "Eaque rerum non.",
                            HotelName = "Reis, Macedo and Silva",
                            InitialRoomPrice = 464.837547174619600m,
                            Location = "Santa Maria",
                            Stars = (byte)2,
                            Tags = "recusandae,rerum,dolorem",
                            ZipCode = "55138-180"
                        },
                        new
                        {
                            HotelId = 6L,
                            Description = "Consequatur dignissimos quae provident consequatur sit.",
                            HotelName = "Pereira - Carvalho",
                            InitialRoomPrice = 431.556584933894500m,
                            Location = "Santa Maria",
                            Stars = (byte)2,
                            Tags = "corrupti,consequatur,voluptates",
                            ZipCode = "10667-577"
                        },
                        new
                        {
                            HotelId = 7L,
                            Description = "Quia aliquid vel voluptas maiores quas voluptates vitae.",
                            HotelName = "Souza Comércio",
                            InitialRoomPrice = 323.655926811641200m,
                            Location = "Piracicaba",
                            Stars = (byte)3,
                            Tags = "facilis,expedita,consectetur",
                            ZipCode = "18930-070"
                        },
                        new
                        {
                            HotelId = 8L,
                            Description = "Autem ut et temporibus eum fuga voluptas.",
                            HotelName = "Braga LTDA",
                            InitialRoomPrice = 655.616111567429800m,
                            Location = "Camaçari",
                            Stars = (byte)5,
                            Tags = "id,rem,quia",
                            ZipCode = "08953-819"
                        },
                        new
                        {
                            HotelId = 9L,
                            Description = "Quis laudantium dolore aliquid aut nesciunt quae deleniti beatae.",
                            HotelName = "Macedo - Carvalho",
                            InitialRoomPrice = 375.546631399300m,
                            Location = "Guarujá",
                            Stars = (byte)1,
                            Tags = "veniam,consequatur,laudantium",
                            ZipCode = "15912-474"
                        },
                        new
                        {
                            HotelId = 10L,
                            Description = "Voluptatem perferendis rem voluptas et.",
                            HotelName = "Pereira, Carvalho and Batista",
                            InitialRoomPrice = 191.392651824059800m,
                            Location = "Goiânia",
                            Stars = (byte)4,
                            Tags = "incidunt,culpa,illum",
                            ZipCode = "31400-748"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
