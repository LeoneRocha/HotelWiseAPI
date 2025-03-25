using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class ChatSessionHistoryv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ChatSessionHistory",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "São Luís", new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(240), 868.58489977034500m, "Rodovia 58149 Moreira Rua", new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(241), (byte)2, "BA", "79353-861" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1657), new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1657), new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1658), new byte[] { 106, 115, 134, 212, 174, 26, 202, 120, 210, 33, 215, 183, 219, 120, 52, 22, 63, 247, 91, 65, 132, 255, 93, 150, 174, 10, 94, 142, 123, 28, 240, 101, 252, 124, 151, 31, 157, 35, 215, 96, 92, 174, 93, 85, 204, 104, 109, 45, 129, 111, 197, 253, 98, 106, 117, 137, 1, 249, 191, 50, 219, 76, 15, 4 }, new byte[] { 107, 48, 10, 17, 199, 68, 13, 62, 133, 159, 131, 158, 170, 41, 124, 240, 174, 165, 23, 202, 241, 207, 56, 5, 126, 219, 49, 90, 30, 192, 206, 218, 149, 11, 51, 140, 204, 139, 39, 132, 30, 50, 132, 205, 153, 238, 253, 93, 208, 54, 134, 20, 149, 218, 131, 37, 100, 222, 87, 124, 35, 13, 225, 125, 171, 207, 56, 133, 135, 16, 62, 22, 133, 234, 148, 217, 205, 251, 246, 192, 166, 253, 199, 92, 169, 47, 198, 164, 236, 56, 7, 194, 70, 7, 112, 70, 51, 124, 144, 186, 153, 44, 212, 95, 66, 125, 186, 132, 59, 215, 90, 47, 15, 22, 107, 126, 45, 232, 47, 143, 87, 112, 115, 69, 230, 26, 113, 255 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ChatSessionHistory");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Mossoró", new DateTime(2025, 3, 25, 2, 3, 8, 950, DateTimeKind.Utc).AddTicks(8650), 705.009977146275400m, "Travessa 2930 Nataniel Marginal", new DateTime(2025, 3, 25, 2, 3, 8, 950, DateTimeKind.Utc).AddTicks(8652), (byte)4, "MT", "12139-521" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 25, 2, 3, 8, 950, DateTimeKind.Utc).AddTicks(9978), new DateTime(2025, 3, 25, 2, 3, 8, 950, DateTimeKind.Utc).AddTicks(9979), new DateTime(2025, 3, 25, 2, 3, 8, 950, DateTimeKind.Utc).AddTicks(9979), new byte[] { 205, 101, 120, 8, 92, 243, 139, 132, 214, 168, 153, 21, 213, 242, 78, 247, 96, 246, 211, 20, 254, 232, 220, 248, 3, 225, 253, 39, 136, 125, 116, 12, 109, 14, 12, 101, 13, 58, 39, 135, 131, 97, 177, 43, 22, 84, 80, 109, 158, 75, 104, 199, 19, 165, 94, 89, 21, 103, 26, 238, 174, 7, 216, 54 }, new byte[] { 217, 176, 69, 193, 14, 43, 193, 173, 248, 219, 11, 137, 169, 247, 12, 228, 231, 20, 178, 166, 85, 212, 16, 121, 126, 15, 149, 30, 23, 242, 60, 142, 166, 163, 99, 217, 237, 19, 73, 54, 166, 178, 93, 132, 63, 208, 84, 48, 57, 212, 3, 103, 150, 151, 109, 8, 18, 62, 18, 36, 30, 114, 70, 198, 145, 30, 48, 88, 6, 28, 42, 141, 97, 62, 137, 198, 59, 34, 245, 156, 76, 125, 229, 120, 29, 175, 73, 176, 254, 26, 198, 13, 107, 70, 187, 109, 152, 37, 253, 89, 169, 164, 1, 128, 16, 138, 200, 212, 57, 63, 84, 144, 181, 133, 134, 193, 248, 164, 41, 92, 223, 171, 56, 188, 225, 77, 74, 153 } });
        }
    }
}
