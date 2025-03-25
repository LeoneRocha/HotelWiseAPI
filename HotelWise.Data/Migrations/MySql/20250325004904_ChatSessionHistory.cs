using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class ChatSessionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatSessionHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdToken = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    PromptMessageHistory = table.Column<string>(type: "text", maxLength: 65535, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    CountMessages = table.Column<int>(type: "int", nullable: false),
                    SessionDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdUser = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessionHistory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1");

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

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessionHistory_SessionDateTime",
                table: "ChatSessionHistory",
                column: "SessionDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatSessionHistory");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Santo André", new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2547), 987.81193357016500m, "Avenida 07287 Martins Avenida", new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(2550), (byte)2, "PI", "31481-061" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4545), new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546), new DateTime(2024, 11, 20, 19, 56, 47, 73, DateTimeKind.Utc).AddTicks(4546), new byte[] { 216, 63, 57, 138, 3, 180, 164, 1, 225, 4, 52, 170, 164, 223, 203, 136, 68, 137, 127, 139, 221, 172, 247, 18, 25, 192, 246, 194, 139, 164, 161, 56, 107, 134, 115, 95, 248, 62, 10, 150, 248, 53, 121, 37, 87, 132, 183, 216, 48, 56, 111, 15, 27, 137, 124, 56, 86, 95, 21, 17, 15, 193, 124, 95 }, new byte[] { 180, 226, 18, 138, 49, 114, 199, 121, 145, 232, 124, 229, 135, 189, 158, 215, 190, 20, 187, 174, 181, 48, 168, 141, 30, 66, 25, 136, 175, 12, 138, 59, 48, 14, 131, 71, 163, 42, 68, 220, 251, 70, 128, 154, 180, 77, 241, 35, 235, 223, 161, 27, 135, 13, 67, 5, 122, 77, 122, 198, 125, 203, 151, 205, 2, 169, 121, 212, 187, 124, 110, 179, 236, 193, 15, 181, 249, 98, 42, 121, 59, 93, 90, 60, 158, 64, 224, 205, 182, 48, 47, 195, 78, 23, 49, 122, 203, 121, 6, 121, 102, 133, 239, 98, 56, 93, 202, 168, 10, 56, 7, 100, 27, 139, 121, 151, 36, 222, 79, 123, 20, 92, 104, 188, 107, 140, 143, 228 } });
        }
    }
}
