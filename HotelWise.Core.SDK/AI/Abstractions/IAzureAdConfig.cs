namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração Azure AD / Microsoft Entra ID.
/// Usado para autenticação OAuth/OIDC em cenários em que a API de IA
/// ou recursos Azure exigem identidade corporativa.
/// </summary>
public interface IAzureAdConfig
{
    /// <summary>
    /// Audience (recurso) esperada nos tokens JWT.
    /// </summary>
    string Audience { get; set; }

    /// <summary>
    /// Caminho de callback após autenticação interativa.
    /// </summary>
    string CallbackPath { get; set; }

    /// <summary>
    /// Identificador do aplicativo (Client ID) registrado no Entra ID.
    /// </summary>
    string ClientId { get; set; }

    /// <summary>
    /// Segredo do cliente usado no fluxo confidencial.
    /// </summary>
    string ClientSecret { get; set; }

    /// <summary>
    /// Domínio do diretório Azure AD.
    /// </summary>
    string Domain { get; set; }

    /// <summary>
    /// URL base da instância do Entra ID (ex.: login.microsoftonline.com).
    /// </summary>
    string Instance { get; set; }

    /// <summary>
    /// Escopos OAuth solicitados na autenticação.
    /// </summary>
    string Scopes { get; set; }

    /// <summary>
    /// Caminho de callback após sign-out.
    /// </summary>
    string SignedOutCallbackPath { get; set; }

    /// <summary>
    /// Identificador do tenant (diretório) Azure AD.
    /// </summary>
    string TenantId { get; set; }
}
