#if NET8_0_OR_GREATER
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Helpers;
using Microsoft.IdentityModel.Tokens;
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Utilitários de segurança locais (sem SCH Ported): hash HMAC-SHA512, JWT legado e Base64.
/// </summary>
[SdkWrappedSource(targetType: "HotelWise.Domain.Helpers.PasswordHashHelper", targetPackage: "HotelWise.Domain", description: "Casca local HMAC-SHA512 + JWT legado sem SCH Ported.")]
public static class SecurityHelper
{
    /// <summary>Cria hash e salt seguros para a senha informada usando HMAC-SHA512.</summary>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>Valida a senha contra o hash e salt armazenados.</summary>
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

    /// <summary>Gera JWT HMAC-SHA256 legado a partir de <see cref="SecurityDto"/>.</summary>
    public static string CreateToken(SecurityDto secVo)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, secVo.Id),
            new(ClaimTypes.Name, secVo.Name),
            new(ClaimTypes.Role, secVo.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secVo.SecurityKeyConfig));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DataHelper.GetDateTimeNow().AddDays(1),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    /// <summary>Verifica se a string informada é uma codificação Base64 válida.</summary>
    public static bool IsBase64String(string base64)
    {
        if (string.IsNullOrEmpty(base64))
        {
            return false;
        }

        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}
#endif
