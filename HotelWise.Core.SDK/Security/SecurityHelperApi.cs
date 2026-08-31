using System.Security.Claims;

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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class SecurityHelperApi
{
    public static long GetUserIdApi(ClaimsPrincipal user) =>
        SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi.GetUserIdApi(user);
}
