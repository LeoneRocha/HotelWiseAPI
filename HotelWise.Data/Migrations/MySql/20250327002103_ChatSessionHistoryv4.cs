using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class ChatSessionHistoryv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalTokensMessage",
                table: "ChatSessionHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "StateCode", "ZipCode" },
                values: new object[] { "Campos dos Goytacazes", new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(7144), 997.39715217676300m, "Rodovia 5874 Morgana Marginal", new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(7146), "ES", "20268-591" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8558), new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8559), new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8559), new byte[] { 212, 24, 227, 229, 204, 86, 105, 98, 35, 243, 113, 170, 14, 108, 1, 183, 201, 33, 122, 124, 134, 170, 200, 253, 245, 201, 219, 17, 31, 182, 171, 167, 141, 53, 104, 21, 46, 143, 20, 140, 149, 177, 228, 242, 66, 175, 175, 169, 166, 7, 89, 127, 191, 53, 236, 197, 105, 229, 143, 60, 87, 125, 100, 187 }, new byte[] { 29, 186, 198, 52, 100, 14, 163, 127, 201, 75, 190, 112, 80, 122, 29, 252, 112, 152, 175, 206, 253, 84, 158, 179, 166, 100, 98, 102, 178, 21, 111, 33, 1, 166, 196, 14, 0, 73, 146, 98, 178, 149, 146, 140, 98, 25, 104, 104, 149, 244, 10, 208, 181, 249, 205, 89, 1, 175, 21, 160, 8, 84, 110, 126, 147, 221, 172, 144, 196, 12, 181, 118, 101, 231, 217, 163, 122, 92, 109, 60, 164, 173, 188, 25, 105, 251, 234, 200, 200, 12, 103, 205, 36, 198, 231, 192, 43, 163, 168, 66, 27, 122, 24, 13, 137, 17, 114, 126, 231, 223, 87, 74, 214, 208, 1, 130, 84, 250, 59, 187, 93, 86, 49, 192, 18, 255, 14, 140 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTokensMessage",
                table: "ChatSessionHistory");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "StateCode", "ZipCode" },
                values: new object[] { "São Luís", new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(240), 868.58489977034500m, "Rodovia 58149 Moreira Rua", new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(241), "BA", "79353-861" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1657), new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1657), new DateTime(2025, 3, 25, 17, 19, 51, 931, DateTimeKind.Utc).AddTicks(1658), new byte[] { 106, 115, 134, 212, 174, 26, 202, 120, 210, 33, 215, 183, 219, 120, 52, 22, 63, 247, 91, 65, 132, 255, 93, 150, 174, 10, 94, 142, 123, 28, 240, 101, 252, 124, 151, 31, 157, 35, 215, 96, 92, 174, 93, 85, 204, 104, 109, 45, 129, 111, 197, 253, 98, 106, 117, 137, 1, 249, 191, 50, 219, 76, 15, 4 }, new byte[] { 107, 48, 10, 17, 199, 68, 13, 62, 133, 159, 131, 158, 170, 41, 124, 240, 174, 165, 23, 202, 241, 207, 56, 5, 126, 219, 49, 90, 30, 192, 206, 218, 149, 11, 51, 140, 204, 139, 39, 132, 30, 50, 132, 205, 153, 238, 253, 93, 208, 54, 134, 20, 149, 218, 131, 37, 100, 222, 87, 124, 35, 13, 225, 125, 171, 207, 56, 133, 135, 16, 62, 22, 133, 234, 148, 217, 205, 251, 246, 192, 166, 253, 199, 92, 169, 47, 198, 164, 236, 56, 7, 194, 70, 7, 112, 70, 51, 124, 144, 186, 153, 44, 212, 95, 66, 125, 186, 132, 59, 215, 90, 47, 15, 22, 107, 126, 45, 232, 47, 143, 87, 112, 115, 69, 230, 26, 113, 255 } });
        }
    }
}
