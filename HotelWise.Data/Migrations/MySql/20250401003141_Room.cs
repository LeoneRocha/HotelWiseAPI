using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Room : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HotelId = table.Column<long>(type: "bigint", nullable: false),
                    RoomType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Capacity = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MinimumNights = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_User_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Room_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "RoomAvailability",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    AvailabilityWithPrice = table.Column<string>(type: "text", maxLength: 65535, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomAvailability_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1");

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

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CheckInDate",
                table: "Reservation",
                column: "CheckInDate");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_CheckOutDate",
                table: "Reservation",
                column: "CheckOutDate");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationDate",
                table: "Reservation",
                column: "ReservationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RoomId",
                table: "Reservation",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_CreatedUserId",
                table: "Room",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_HotelId",
                table: "Room",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_ModifyUserId",
                table: "Room",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomType",
                table: "Room",
                column: "RoomType");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailability_EndDate",
                table: "RoomAvailability",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailability_RoomId",
                table: "RoomAvailability",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAvailability_StartDate",
                table: "RoomAvailability",
                column: "StartDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "RoomAvailability");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 1L,
                columns: new[] { "City", "CreatedDate", "InitialRoomPrice", "Location", "ModifyDate", "Stars", "StateCode", "ZipCode" },
                values: new object[] { "Campos dos Goytacazes", new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(7144), 997.39715217676300m, "Rodovia 5874 Morgana Marginal", new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(7146), (byte)2, "ES", "20268-591" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "LastAccessDate", "ModifyDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8558), new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8559), new DateTime(2025, 3, 27, 0, 21, 3, 29, DateTimeKind.Utc).AddTicks(8559), new byte[] { 212, 24, 227, 229, 204, 86, 105, 98, 35, 243, 113, 170, 14, 108, 1, 183, 201, 33, 122, 124, 134, 170, 200, 253, 245, 201, 219, 17, 31, 182, 171, 167, 141, 53, 104, 21, 46, 143, 20, 140, 149, 177, 228, 242, 66, 175, 175, 169, 166, 7, 89, 127, 191, 53, 236, 197, 105, 229, 143, 60, 87, 125, 100, 187 }, new byte[] { 29, 186, 198, 52, 100, 14, 163, 127, 201, 75, 190, 112, 80, 122, 29, 252, 112, 152, 175, 206, 253, 84, 158, 179, 166, 100, 98, 102, 178, 21, 111, 33, 1, 166, 196, 14, 0, 73, 146, 98, 178, 149, 146, 140, 98, 25, 104, 104, 149, 244, 10, 208, 181, 249, 205, 89, 1, 175, 21, 160, 8, 84, 110, 126, 147, 221, 172, 144, 196, 12, 181, 118, 101, 231, 217, 163, 122, 92, 109, 60, 164, 173, 188, 25, 105, 251, 234, 200, 200, 12, 103, 205, 36, 198, 231, 192, 43, 163, 168, 66, 27, 122, 24, 13, 137, 17, 114, 126, 231, 223, 87, 74, 214, 208, 1, 130, 84, 250, 59, 187, 93, 86, 49, 192, 18, 255, 14, 140 } });
        }
    }
}
