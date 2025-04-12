using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Roomv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Room",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "São Vicente", new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(3290), 812.006730389325700m, "Travessa 5491 Carvalho Travessa", new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(3292), (byte)5, "SP", "55093-317" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4733), new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734), new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734), new byte[] { 30, 247, 136, 135, 205, 208, 14, 65, 141, 142, 181, 85, 147, 48, 124, 124, 134, 40, 66, 225, 229, 191, 214, 55, 79, 103, 1, 108, 249, 128, 134, 250, 172, 128, 32, 251, 235, 95, 123, 61, 97, 173, 246, 119, 125, 72, 47, 190, 75, 21, 125, 23, 7, 122, 29, 48, 189, 54, 82, 80, 16, 99, 153, 232 }, new byte[] { 53, 90, 45, 101, 238, 159, 120, 237, 139, 200, 29, 239, 128, 104, 208, 207, 79, 158, 115, 110, 97, 89, 54, 25, 231, 145, 242, 195, 242, 15, 191, 169, 73, 192, 184, 78, 117, 230, 89, 45, 128, 46, 63, 83, 58, 99, 83, 168, 100, 211, 64, 163, 238, 14, 69, 80, 157, 146, 254, 137, 175, 120, 148, 50, 122, 17, 115, 207, 223, 92, 128, 5, 195, 211, 90, 219, 206, 201, 242, 69, 207, 202, 254, 84, 219, 22, 57, 124, 199, 208, 91, 146, 234, 202, 61, 67, 71, 75, 46, 93, 44, 35, 211, 127, 249, 235, 231, 179, 72, 176, 2, 26, 47, 89, 73, 42, 85, 69, 189, 82, 142, 234, 59, 69, 202, 65, 30, 250 } });

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailability_Currency",
                table: "RoomAvailability",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailability_RoomId_StartDate_EndDate_Currency",
                table: "RoomAvailability",
                columns: new[] { "RoomId", "StartDate", "EndDate", "Currency" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomAvailability_Currency",
                table: "RoomAvailability");

            migrationBuilder.DropIndex(
                name: "IX_RoomAvailability_RoomId_StartDate_EndDate_Currency",
                table: "RoomAvailability");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Room");

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
    }
}
