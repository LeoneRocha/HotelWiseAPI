using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class ChatSessionHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatSessionHistory_IdToken",
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

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessionHistory_IdToken",
                table: "ChatSessionHistory",
                column: "IdToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatSessionHistory_IdToken",
                table: "ChatSessionHistory");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Betim", new DateTime(2025, 3, 25, 0, 49, 4, 556, DateTimeKind.Utc).AddTicks(9795), 936.373348238649400m, "Rodovia 0871 Dalila Travessa", new DateTime(2025, 3, 25, 0, 49, 4, 556, DateTimeKind.Utc).AddTicks(9798), (byte)3, "PE", "93344-026" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 25, 0, 49, 4, 557, DateTimeKind.Utc).AddTicks(1221), new DateTime(2025, 3, 25, 0, 49, 4, 557, DateTimeKind.Utc).AddTicks(1222), new DateTime(2025, 3, 25, 0, 49, 4, 557, DateTimeKind.Utc).AddTicks(1223), new byte[] { 219, 87, 150, 128, 12, 120, 44, 219, 14, 31, 212, 179, 199, 49, 107, 227, 228, 31, 106, 196, 195, 196, 172, 167, 48, 50, 9, 33, 143, 183, 172, 201, 112, 54, 59, 73, 173, 5, 45, 117, 129, 210, 197, 33, 147, 138, 188, 0, 74, 77, 10, 108, 93, 140, 56, 246, 22, 176, 226, 5, 174, 19, 127, 204 }, new byte[] { 123, 212, 79, 194, 2, 189, 86, 146, 156, 162, 201, 119, 104, 147, 244, 37, 41, 200, 184, 104, 165, 254, 229, 17, 124, 134, 241, 198, 23, 221, 248, 23, 179, 97, 109, 238, 125, 248, 42, 80, 43, 208, 91, 171, 113, 23, 26, 148, 95, 151, 69, 221, 83, 47, 160, 152, 192, 250, 235, 10, 124, 32, 113, 155, 26, 82, 48, 225, 187, 147, 76, 138, 225, 214, 217, 18, 219, 54, 141, 196, 229, 178, 172, 232, 230, 241, 191, 173, 81, 38, 109, 222, 135, 184, 56, 205, 3, 191, 72, 159, 239, 145, 47, 189, 11, 155, 88, 32, 229, 3, 181, 115, 171, 97, 72, 73, 237, 121, 16, 48, 118, 128, 101, 5, 28, 81, 12, 168 } });

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessionHistory_IdToken",
                table: "ChatSessionHistory",
                column: "IdToken",
                unique: true);
        }
    }
}
