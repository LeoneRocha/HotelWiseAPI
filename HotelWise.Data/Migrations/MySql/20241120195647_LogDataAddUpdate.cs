using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class LogDataAddUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Hotels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedUserId",
                table: "Hotels",
                type: "bigint",
                nullable: true,
                defaultValue: 1L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Hotels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModifyUserId",
                table: "Hotels",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "CreatedUserId", "InitialRoomPrice", "Location", "ModifyDate", "ModifyUserId", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Santo André", new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2547), 1L, 987.81193357016500m, "Avenida 07287 Martins Avenida", new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2550), 1L, (byte)2, "PI", "31481-061" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4545), new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546), new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546), new byte[] { 216, 63, 57, 138, 3, 180, 164, 1, 225, 4, 52, 170, 164, 223, 203, 136, 68, 137, 127, 139, 221, 172, 247, 18, 25, 192, 246, 194, 139, 164, 161, 56, 107, 134, 115, 95, 248, 62, 10, 150, 248, 53, 121, 37, 87, 132, 183, 216, 48, 56, 111, 15, 27, 137, 124, 56, 86, 95, 21, 17, 15, 193, 124, 95 }, new byte[] { 180, 226, 18, 138, 49, 114, 199, 121, 145, 232, 124, 229, 135, 189, 158, 215, 190, 20, 187, 174, 181, 48, 168, 141, 30, 66, 25, 136, 175, 12, 138, 59, 48, 14, 131, 71, 163, 42, 68, 220, 251, 70, 128, 154, 180, 77, 241, 35, 235, 223, 161, 27, 135, 13, 67, 5, 122, 77, 122, 198, 125, 203, 151, 205, 2, 169, 121, 212, 187, 124, 110, 179, 236, 193, 15, 181, 249, 98, 42, 121, 59, 93, 90, 60, 158, 64, 224, 205, 182, 48, 47, 195, 78, 23, 49, 122, 203, 121, 6, 121, 102, 133, 239, 98, 56, 93, 202, 168, 10, 56, 7, 100, 27, 139, 121, 151, 36, 222, 79, 123, 20, 92, 104, 188, 107, 140, 143, 228 } });

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CreatedUserId",
                table: "Hotels",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ModifyUserId",
                table: "Hotels",
                column: "ModifyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_User_CreatedUserId",
                table: "Hotels",
                column: "CreatedUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_User_ModifyUserId",
                table: "Hotels",
                column: "ModifyUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_User_CreatedUserId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_User_ModifyUserId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_CreatedUserId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ModifyUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ModifyUserId",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "InitialRoomPrice", "Location", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Manaus", 635.576826273363100m, "Rodovia 702 Xavier Travessa", (byte)1, "SP", "26237-630" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1204), new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207), new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207), new byte[] { 187, 83, 45, 107, 158, 13, 62, 226, 68, 182, 45, 0, 201, 112, 98, 94, 45, 228, 139, 155, 28, 157, 176, 65, 30, 1, 229, 151, 203, 209, 4, 9, 20, 229, 129, 253, 28, 201, 80, 30, 237, 142, 137, 239, 168, 242, 228, 0, 159, 111, 63, 128, 62, 74, 77, 218, 64, 163, 118, 239, 215, 154, 73, 217 }, new byte[] { 155, 141, 190, 210, 100, 225, 47, 238, 222, 121, 89, 19, 132, 244, 147, 250, 128, 75, 239, 154, 142, 62, 79, 121, 254, 115, 171, 164, 120, 216, 29, 216, 142, 16, 143, 181, 76, 201, 250, 87, 56, 104, 140, 27, 115, 127, 229, 227, 229, 111, 234, 209, 232, 61, 93, 242, 217, 87, 41, 91, 91, 8, 197, 152, 118, 70, 58, 2, 2, 133, 115, 223, 238, 129, 136, 240, 91, 6, 225, 227, 152, 177, 8, 21, 94, 13, 75, 235, 104, 221, 241, 209, 203, 92, 29, 132, 54, 215, 131, 232, 185, 16, 94, 151, 59, 226, 38, 63, 211, 100, 173, 184, 47, 13, 208, 165, 15, 175, 136, 209, 180, 47, 118, 1, 155, 246, 196, 27 } });
        }
    }
}
