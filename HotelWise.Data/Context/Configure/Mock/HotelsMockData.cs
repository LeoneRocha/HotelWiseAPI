using Bogus;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Data.Context.Configure.Mock
{
    public static class HotelsMockData
    {
        private static readonly string[] _tags = { "Luxury", "Spa" };

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
                Tags = _tags,
                Stars = (byte)faker.Random.Int(1, 5),
                InitialRoomPrice = faker.Random.Decimal(100, 1000),
                ZipCode = hotelAddress.ZipCode(),
                Location = $"{hotelAddress.StreetSuffix()} {hotelAddress.StreetAddress()}",
                City = hotelAddress.City(),
                StateCode = hotelAddress.StateAbbr(),
                CreatedUserId = 1,
                ModifyUserId = 1,   
                CreatedDate = DateTime.UtcNow,
                ModifyDate = DateTime.UtcNow
            }); 
            return hotels.ToArray();
        } 
    }
}