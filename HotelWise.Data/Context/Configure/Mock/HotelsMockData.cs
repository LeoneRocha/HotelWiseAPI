using Bogus;
using HotelWise.Domain.Model;

namespace HotelWise.Data.Context.Configure.Mock
{
    public static class HotelsMockData
    {
        public static Hotel[] GetHotels()
        {
            var faker = new Faker("pt_BR");
            var hotels = new List<Hotel>();

            var hotelAddress = faker.Address;

            hotels.Add(new Hotel
            {
                HotelId = 1,
                HotelName = "Hotel Example",
                Description = "An example hotel",
                Tags = new[] { "Luxury", "Spa" },
                Stars = (byte)faker.Random.Int(1, 5),
                InitialRoomPrice = faker.Random.Decimal(100, 1000),
                ZipCode = hotelAddress.ZipCode(),
                Location = $"{hotelAddress.StreetSuffix()} {hotelAddress.StreetAddress()}",
                City = hotelAddress.City(),
                StateCode = hotelAddress.StateAbbr(),
            });

            return hotels.ToArray();
        }

        private static void GetHotelsGenerateFromBogus(Faker faker, List<Hotel> hotels)
        { 
            for (int i = 2; i <= 10; i++)
            {
                hotels.Add(new Hotel
                {
                    HotelId = i,
                    HotelName = faker.Company.CompanyName(),
                    Description = faker.Lorem.Sentence(),
                    Tags = faker.Lorem.Words(3).ToArray(),
                    Stars = (byte)faker.Random.Int(1, 5),
                    InitialRoomPrice = faker.Random.Decimal(100, 1000),
                    ZipCode = faker.Address.ZipCode(),
                    Location = faker.Address.City()
                });
            } 
        }
    }
}