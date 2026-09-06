using System.Text;

namespace HotelWise.Domain.Helpers;

/// <summary>
/// Hash/verificação de senha HMAC-SHA512 (byte[]) — mesmo algoritmo legado HW.
/// Não migrar storage de senha sem migration de dados.
/// </summary>
public static class PasswordHashHelper
{
    /// <summary>Gera hash e salt HMAC-SHA512 para a senha informada.</summary>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>Verifica se a senha corresponde ao hash/salt HMAC-SHA512 informados.</summary>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}
