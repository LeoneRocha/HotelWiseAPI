using System.Security.Claims;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Utilitários de segurança voltados a APIs ASP.NET Core:
/// extração do identificador do usuário autenticado a partir de <see cref="ClaimsPrincipal"/>.
/// </summary>
/// <example>
/// <code>
/// long userId = SecurityHelperApi.GetUserIdApi(HttpContext.User);
/// </code>
/// </example>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi em SmartCoreHub.Core.SDK.")]
public static class SecurityHelperApi
{
    /// <summary>Extrai o ID numérico do usuário a partir dos claims do token JWT.</summary>
    /// <param name="user">Principal do usuário autenticado.</param>
    /// <returns>Identificador do usuário como long.</returns>
    public static long GetUserIdApi(ClaimsPrincipal user) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi.GetUserIdApi(user);
}
