namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Value object imutável (após construção) que representa o resultado de autenticação:
/// flag de autenticado, timestamps e pares access/refresh token.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Security.TokenVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TokenVO : SmartCoreHub.Core.SDK.Common.Security.TokenVO
{
    public TokenVO()
    {
    }

    public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
        : base(authenticated, created, expiration, accessToken, refreshToken)
    {
    }
}
