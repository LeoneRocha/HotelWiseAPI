#if NET8_0_OR_GREATER
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Utilitários de segurança: hash/verificação de senhas (HMAC-SHA512),
/// geração simplificada de JWT a partir de <see cref="SecurityDto"/> e
/// validação de strings Base64.
/// </summary>
/// <example>
/// <code>
/// SecurityHelper.CreatePasswordHash(pwd, out var hash, out var salt);
/// bool ok = SecurityHelper.VerifyPasswordHash(pwd, hash, salt);
/// string jwt = SecurityHelper.CreateToken(securityDto);
/// </code>
/// </example>
public static class SecurityHelper
{
    /// <summary>
    /// Gera hash e salt de senha com HMAC-SHA512.
    /// </summary>
    /// <param name="password">Senha em texto claro.</param>
    /// <param name="passwordHash">Hash resultante (out).</param>
    /// <param name="passwordSalt">Salt (chave HMAC) resultante (out).</param>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    /// <summary>
    /// Verifica se a senha corresponde ao hash/salt armazenados.
    /// </summary>
    /// <param name="password">Senha em texto claro a verificar.</param>
    /// <param name="passwordHash">Hash esperado.</param>
    /// <param name="passwordSalt">Salt usado na geração do hash.</param>
    /// <returns><c>true</c> se a senha for válida; caso contrário, <c>false</c>.</returns>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
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

    /// <summary>
    /// Cria um JWT (HMAC-SHA256) com claims de Id, Name e Role a partir de <see cref="SecurityDto"/>.
    /// </summary>
    /// <param name="secVo">Dados de segurança e chave de assinatura.</param>
    /// <returns>Token JWT serializado com validade de 1 dia.</returns>
    public static string CreateToken(SecurityDto secVo)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, secVo.Id),
            new Claim(ClaimTypes.Name, secVo.Name),
            new Claim(ClaimTypes.Role, secVo.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secVo.SecurityKeyConfig));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokendDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DataHelper.GetDateTimeNow().AddDays(1),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokendDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Indica se a string é Base64 válida.
    /// </summary>
    /// <param name="base64">Texto a validar.</param>
    /// <returns><c>true</c> se for Base64 decodificável; caso contrário, <c>false</c>.</returns>
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
