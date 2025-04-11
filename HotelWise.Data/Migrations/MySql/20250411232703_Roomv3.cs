using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Roomv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "RoomAvailability",
                type: "varchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Santos", new DateTime(2025, 4, 11, 23, 27, 2, 746, DateTimeKind.Utc).AddTicks(6777), 862.355039008807900m, "Avenida 750 Lara Rodovia", new DateTime(2025, 4, 11, 23, 27, 2, 746, DateTimeKind.Utc).AddTicks(6779), (byte)3, "DF", "27679-827" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 4, 11, 23, 27, 2, 746, DateTimeKind.Utc).AddTicks(8205), new DateTime(2025, 4, 11, 23, 27, 2, 746, DateTimeKind.Utc).AddTicks(8205), new DateTime(2025, 4, 11, 23, 27, 2, 746, DateTimeKind.Utc).AddTicks(8206), new byte[] { 189, 156, 134, 157, 251, 122, 57, 82, 185, 255, 170, 85, 104, 0, 185, 180, 222, 240, 230, 30, 9, 55, 231, 35, 161, 241, 230, 161, 115, 148, 16, 64, 242, 163, 164, 203, 224, 25, 128, 222, 112, 188, 162, 153, 126, 248, 210, 19, 94, 32, 37, 210, 115, 178, 243, 71, 248, 191, 118, 214, 160, 122, 120, 132 }, new byte[] { 87, 124, 27, 240, 109, 16, 241, 196, 217, 78, 6, 21, 117, 101, 147, 19, 159, 140, 172, 69, 253, 10, 174, 103, 233, 172, 136, 149, 245, 195, 184, 211, 192, 122, 213, 31, 87, 153, 101, 153, 246, 130, 139, 75, 166, 20, 147, 188, 201, 70, 247, 121, 22, 254, 125, 115, 134, 122, 198, 154, 233, 150, 174, 74, 250, 100, 139, 83, 243, 169, 159, 60, 94, 192, 243, 30, 35, 2, 172, 210, 99, 217, 197, 144, 201, 89, 246, 60, 199, 62, 54, 107, 139, 126, 140, 76, 156, 160, 149, 74, 215, 107, 75, 158, 174, 255, 60, 110, 86, 145, 21, 3, 69, 78, 45, 24, 209, 18, 221, 125, 164, 130, 123, 103, 16, 75, 137, 13 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "RoomAvailability");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Ribeirão Preto", new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(8544), 404.313868838800900m, "Travessa 1398 Lavínia Avenida", new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(8545), (byte)2, "TO", "05086-749" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9903), new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9903), new DateTime(2025, 4, 2, 23, 50, 49, 858, DateTimeKind.Utc).AddTicks(9904), new byte[] { 134, 89, 65, 106, 93, 229, 198, 160, 66, 6, 148, 69, 222, 213, 210, 215, 159, 152, 53, 36, 132, 97, 203, 222, 211, 153, 24, 222, 224, 93, 69, 16, 29, 9, 145, 241, 106, 10, 82, 205, 73, 36, 40, 18, 203, 9, 174, 16, 137, 247, 211, 165, 6, 210, 190, 184, 236, 222, 59, 158, 252, 163, 201, 113 }, new byte[] { 44, 226, 248, 24, 117, 208, 254, 115, 9, 159, 52, 77, 233, 6, 52, 233, 18, 188, 45, 237, 239, 217, 219, 187, 48, 87, 5, 191, 57, 144, 148, 66, 9, 148, 10, 200, 176, 71, 167, 8, 97, 255, 106, 65, 199, 172, 15, 190, 6, 168, 61, 182, 112, 55, 137, 15, 198, 213, 31, 61, 23, 181, 81, 80, 158, 60, 9, 38, 174, 38, 234, 102, 82, 141, 119, 158, 72, 85, 31, 49, 225, 249, 177, 53, 174, 126, 42, 12, 49, 117, 59, 238, 246, 51, 147, 28, 61, 1, 87, 174, 16, 69, 155, 170, 231, 15, 127, 14, 153, 213, 106, 21, 55, 225, 184, 76, 27, 119, 167, 165, 253, 63, 93, 233, 79, 47, 176, 238 } });
        }
    }
}
