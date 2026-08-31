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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class SecurityHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

    public static string CreateToken(SecurityDto secVo) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.CreateToken(secVo);

    public static bool IsBase64String(string base64) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper.IsBase64String(base64);
}

#endif
