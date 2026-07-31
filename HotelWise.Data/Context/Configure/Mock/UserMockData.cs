using HotelWise.Domain.Helpers;
using HotelWise.Domain.Model;

namespace HotelWise.Data.Context.Configure.Mock
{
    public static class UserMockData
    {
        private static readonly DateTime SeedCreatedDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4733);
        private static readonly DateTime SeedLastAccessDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734);
        private static readonly DateTime SeedModifyDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734);

        // Senha de teste conhecida: admin123 (hash/salt alinhados à migration FixUserMockSeedPosDotNet10)
        private static readonly byte[] SeedPasswordHash =
        [
            70, 201, 186, 81, 50, 62, 196, 162, 13, 116, 187, 123, 20, 91, 162, 177, 220, 6, 243, 231, 7, 22, 162, 101, 77, 211, 53, 98, 95, 233, 93, 246, 173, 217, 249, 114, 32, 211, 32, 16, 146, 232, 40, 42, 217, 244, 121, 117, 212, 32, 159, 20, 133, 9, 40, 36, 112, 31, 174, 140, 168, 126, 214, 50
        ];

        private static readonly byte[] SeedPasswordSalt =
        [
            118, 163, 138, 243, 56, 33, 187, 238, 154, 10, 102, 122, 98, 90, 84, 134, 89, 107, 231, 74, 202, 165, 254, 23, 22, 178, 94, 231, 165, 206, 15, 192, 155, 167, 13, 246, 107, 138, 221, 201, 208, 119, 137, 243, 241, 186, 88, 223, 253, 63, 101, 90, 73, 246, 216, 87, 176, 204, 150, 123, 71, 210, 223, 42, 90, 194, 84, 238, 185, 198, 250, 93, 236, 24, 48, 203, 26, 66, 173, 106, 150, 190, 3, 98, 79, 89, 133, 115, 78, 22, 101, 231, 223, 103, 44, 176, 183, 71, 146, 234, 187, 208, 36, 81, 85, 79, 187, 36, 46, 12, 206, 205, 25, 225, 53, 126, 120, 92, 51, 206, 194, 47, 4, 67, 10, 53, 95, 56
        ];

        public static User[] GetMock()
        {
            var newAddUser = new User
            {
                Id = 1,
                Name = "User MOCK ",
                Login = "admin",
                Admin = true,
                Email = "admin@sistemas.com",
                CreatedDate = SeedCreatedDate,
                Enable = true,
                LastAccessDate = SeedLastAccessDate,
                ModifyDate = SeedModifyDate,
                Role = "Admin",
                Language = CultureDateTimeHelper.GetCultureBrazil(),
                TimeZone = CultureDateTimeHelper.GetTimeZoneBrazil(),
                PasswordHash = SeedPasswordHash,
                PasswordSalt = SeedPasswordSalt,
                RefreshToken = ""
            };

            return [
                newAddUser
            ];
        }
    }
}
