using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelWise.Data.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Geratehotelsv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelId", "Description", "HotelName", "InitialRoomPrice", "Location", "Stars", "Tags", "ZipCode" },
                values: new object[,]
                {
                    { 2L, "Est aliquid distinctio aut debitis quibusdam ratione.", "Xavier - Barros", 326.553639874863400m, "Santa Maria", (byte)1, "id,natus,ratione", "06785-646" },
                    { 3L, "Fugit libero ex et est officia.", "Albuquerque, Moreira and Barros", 107.37490893956946100m, "Santo André", (byte)4, "ab,omnis,enim", "56242-460" },
                    { 4L, "Vitae sint voluptatem blanditiis et porro a aut.", "Franco LTDA", 231.588574865117200m, "Porto Velho", (byte)4, "esse,neque,est", "11972-342" },
                    { 5L, "Eaque rerum non.", "Reis, Macedo and Silva", 464.837547174619600m, "Santa Maria", (byte)2, "recusandae,rerum,dolorem", "55138-180" },
                    { 6L, "Consequatur dignissimos quae provident consequatur sit.", "Pereira - Carvalho", 431.556584933894500m, "Santa Maria", (byte)2, "corrupti,consequatur,voluptates", "10667-577" },
                    { 7L, "Quia aliquid vel voluptas maiores quas voluptates vitae.", "Souza Comércio", 323.655926811641200m, "Piracicaba", (byte)3, "facilis,expedita,consectetur", "18930-070" },
                    { 8L, "Autem ut et temporibus eum fuga voluptas.", "Braga LTDA", 655.616111567429800m, "Camaçari", (byte)5, "id,rem,quia", "08953-819" },
                    { 9L, "Quis laudantium dolore aliquid aut nesciunt quae deleniti beatae.", "Macedo - Carvalho", 375.546631399300m, "Guarujá", (byte)1, "veniam,consequatur,laudantium", "15912-474" },
                    { 10L, "Voluptatem perferendis rem voluptas et.", "Pereira, Carvalho and Batista", 191.392651824059800m, "Goiânia", (byte)4, "incidunt,culpa,illum", "31400-748" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "HotelId",
                keyValue: 10L);
        }
    }
}
