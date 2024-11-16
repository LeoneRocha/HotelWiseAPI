using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class UserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Login = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Admin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Language = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    TimeZone = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Refresh_token_expiry_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastAccessDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "InitialRoomPrice", "Location", "StateCode", "ZipCode" },
                values: new object[] { "Manaus", 635.576826273363100m, "Rodovia 702 Xavier Travessa", "SP", "26237-630" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Admin", "CreatedDate", "Email", "Enable", "Language", "LastAccessDate", "Login", "ModifyDate", "Name", "PasswordHash", "PasswordSalt", "RefreshToken", "Refresh_token_expiry_time", "Role", "TimeZone" },
                values: new object[] { 1L, true, new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1204), "admin@sistemas.com", true, "pt-BR", new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207), "admin", new DateTime(2024, 11, 16, 22, 41, 59, 142, DateTimeKind.Utc).AddTicks(1207), "User MOCK ", new byte[] { 187, 83, 45, 107, 158, 13, 62, 226, 68, 182, 45, 0, 201, 112, 98, 94, 45, 228, 139, 155, 28, 157, 176, 65, 30, 1, 229, 151, 203, 209, 4, 9, 20, 229, 129, 253, 28, 201, 80, 30, 237, 142, 137, 239, 168, 242, 228, 0, 159, 111, 63, 128, 62, 74, 77, 218, 64, 163, 118, 239, 215, 154, 73, 217 }, new byte[] { 155, 141, 190, 210, 100, 225, 47, 238, 222, 121, 89, 19, 132, 244, 147, 250, 128, 75, 239, 154, 142, 62, 79, 121, 254, 115, 171, 164, 120, 216, 29, 216, 142, 16, 143, 181, 76, 201, 250, 87, 56, 104, 140, 27, 115, 127, 229, 227, 229, 111, 234, 209, 232, 61, 93, 242, 217, 87, 41, 91, 91, 8, 197, 152, 118, 70, 58, 2, 2, 133, 115, 223, 238, 129, 136, 240, 91, 6, 225, 227, 152, 177, 8, 21, 94, 13, 75, 235, 104, 221, 241, 209, 203, 92, 29, 132, 54, 215, 131, 232, 185, 16, 94, 151, 59, 226, 38, 63, 211, 100, 173, 184, 47, 13, 208, 165, 15, 175, 136, 209, 180, 47, 118, 1, 155, 246, 196, 27 }, "", null, "Admin", "E. South America Standard Time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "InitialRoomPrice", "Location", "StateCode", "ZipCode" },
                values: new object[] { "Caucaia", 322.360860039722200m, "Rodovia 2663 Silva Marginal", "MS", "77110-960" });
        }
    }
}
