#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
    /// <summary>Cria hash e salt seguros para a senha informada usando HMAC-SHA512.</summary>
    /// <param name="password">Senha em texto claro.</param>
    /// <param name="passwordHash">Hash gerado de saída.</param>
    /// <param name="passwordSalt">Salt gerado de saída.</param>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

    /// <summary>Valida a senha contra o hash e salt armazenados.</summary>
    /// <param name="password">Senha em texto claro.</param>
    /// <param name="passwordHash">Hash armazenado.</param>
    /// <param name="passwordSalt">Salt armazenado.</param>
    /// <returns>True se a senha for válida; caso contrário false.</returns>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

    /// <summary>Gera uma representação de token a partir das informações de segurança.</summary>
    /// <param name="secVo">Dados de segurança do usuário.</param>
    /// <returns>String do token.</returns>
    public static string CreateToken(SecurityDto secVo) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.CreateToken(secVo);

    /// <summary>Verifica se a string informada é uma codificação Base64 válida.</summary>
    /// <param name="base64">String para teste.</param>
    /// <returns>True se for Base64 válido; caso contrário false.</returns>
    public static bool IsBase64String(string base64) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.IsBase64String(base64);
}
#endif
