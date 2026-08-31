namespace HotelWise.Core.SDK.Security;

/// <summary>
/// Value object imutável (após construção) que representa o resultado de autenticação:
/// flag de autenticado, timestamps e pares access/refresh token.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Security.TokenVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TokenVO
{
    /// <summary>
    /// Inicializa uma instância vazia de <see cref="TokenVO"/>.
    /// </summary>
    public TokenVO()
    {
    }

    /// <summary>
    /// Inicializa o value object com todos os campos de autenticação.
    /// </summary>
    /// <param name="authenticated">Indica se a autenticação foi bem-sucedida.</param>
    /// <param name="created">Momento de criação do token (representação textual).</param>
    /// <param name="expiration">Momento de expiração (representação textual).</param>
    /// <param name="accessToken">JWT de acesso.</param>
    /// <param name="refreshToken">Token de renovação.</param>
    public TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
    {
        Authenticated = authenticated;
        Created = created;
        Expiration = expiration;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    /// <summary>
    /// Indica se o usuário está autenticado.
    /// </summary>
    public bool Authenticated { get; private set; }

    /// <summary>
    /// Data/hora de criação do token (formato textual definido pelo emissor).
    /// </summary>
    public string Created { get; private set; } = string.Empty;

    /// <summary>
    /// Data/hora de expiração do token (formato textual definido pelo emissor).
    /// </summary>
    public string Expiration { get; private set; } = string.Empty;

    /// <summary>
    /// Access token JWT.
    /// </summary>
    public string AccessToken { get; private set; } = string.Empty;

    /// <summary>
    /// Refresh token associado ao access token.
    /// </summary>
    public string RefreshToken { get; private set; } = string.Empty;
}
