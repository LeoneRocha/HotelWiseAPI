using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Room2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "São Vicente", new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(3228), 623.225574471303100m, "Avenida 37023 Costa Rodovia", new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(3230), (byte)1, "CE", "75702-587" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4642), new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4643), new DateTime(2025, 4, 1, 0, 31, 41, 68, DateTimeKind.Utc).AddTicks(4643), new byte[] { 135, 140, 110, 13, 35, 170, 175, 196, 23, 165, 235, 53, 206, 189, 241, 77, 110, 220, 103, 48, 44, 131, 24, 167, 42, 56, 110, 103, 214, 95, 158, 156, 254, 33, 109, 21, 253, 12, 186, 30, 236, 140, 78, 170, 85, 134, 188, 181, 211, 33, 204, 233, 232, 85, 163, 107, 135, 79, 189, 241, 14, 99, 75, 198 }, new byte[] { 57, 41, 246, 85, 73, 51, 206, 77, 116, 78, 111, 63, 103, 97, 211, 170, 140, 171, 28, 59, 203, 158, 9, 233, 106, 167, 85, 198, 213, 249, 135, 197, 11, 64, 167, 137, 105, 156, 172, 63, 103, 235, 221, 23, 94, 229, 156, 41, 23, 157, 42, 195, 32, 179, 245, 171, 4, 188, 45, 41, 134, 0, 160, 4, 233, 189, 165, 4, 229, 214, 179, 65, 81, 210, 87, 89, 242, 208, 28, 210, 117, 94, 120, 148, 28, 75, 239, 237, 237, 167, 141, 184, 67, 32, 180, 77, 223, 30, 61, 68, 86, 219, 2, 54, 54, 182, 27, 22, 244, 152, 87, 177, 2, 187, 245, 19, 153, 207, 8, 107, 3, 109, 153, 244, 66, 191, 221, 69 } });
        }
    }
}
