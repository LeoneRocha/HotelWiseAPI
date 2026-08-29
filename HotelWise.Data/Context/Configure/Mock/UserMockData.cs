using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Model;

namespace HotelWise.Data.Context.Configure.Mock;

/// <summary>
/// Fornece dados padrão (seed/mock) para criação inicial do usuário administrador.
/// </summary>
public static class UserMockData
{
    private static readonly DateTime SeedCreatedDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4733);
    private static readonly DateTime SeedLastAccessDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734);
    private static readonly DateTime SeedModifyDate = new DateTime(2025, 4, 12, 0, 49, 26, 432, DateTimeKind.Utc).AddTicks(4734);
     
    /// <summary>
    /// Retorna a lista de usuários mock pré-configurados com hash de senha e perfil administrativo.
    /// </summary>
    /// <returns>Array de instâncias de <see cref="User"/> para seed do banco.</returns>
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
            TimeZone = CultureDateTimeHelper.GetTimeZoneBrazil()
        };
        SecurityHelper.CreatePasswordHash("admin123", out byte[] passwordHash, out byte[] passwordSalt);
        newAddUser.PasswordHash = passwordHash;
        newAddUser.PasswordSalt = passwordSalt;
         
        return [
            newAddUser
        ];
    }
}
