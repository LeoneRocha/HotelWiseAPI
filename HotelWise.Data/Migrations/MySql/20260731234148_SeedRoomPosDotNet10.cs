using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class SeedRoomPosDotNet10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // HotelId=1 (HasData) may be missing on existing DBs if the seed row was deleted.
            migrationBuilder.Sql("""
                INSERT INTO `Hotels` (`HotelId`, `HotelName`, `Description`, `Tags`, `Stars`, `InitialRoomPrice`, `ZipCode`, `Location`, `City`, `StateCode`, `CreatedUserId`, `ModifyUserId`, `CreatedDate`, `ModifyDate`)
                SELECT 1, 'Hotel Example', 'An example hotel', 'Luxury,Spa', 5, 812.006730389325700, '55093-317', 'Travessa 5491 Carvalho Travessa', 'São Vicente', 'SP', 1, 1, '2025-04-12 00:49:26.4323290', '2025-04-12 00:49:26.4323292'
                FROM DUAL
                WHERE NOT EXISTS (SELECT 1 FROM `Hotels` WHERE `HotelId` = 1);
                """);

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "Capacity", "CreatedDate", "CreatedUserId", "Description", "HotelId", "MinimumNights", "ModifyDate", "ModifyUserId", "Name", "RoomType", "Status" },
                values: new object[] { 100L, (short)2, new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(5000), 1L, "Quarto de exemplo seed pós .NET 10", 1L, (short)1, new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(5001), 1L, "Quarto Example", (byte)2, (byte)1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.Sql("""
                DELETE FROM `Hotels`
                WHERE `HotelId` = 1 AND `HotelName` = 'Hotel Example';
                """);
        }
    }
}
