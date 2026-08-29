namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração Azure AD / Entra ID.
/// </summary>
public interface IAzureAdConfig
{
    string Audience { get; set; }
    string CallbackPath { get; set; }
    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string Domain { get; set; }
    string Instance { get; set; }
    string Scopes { get; set; }
    string SignedOutCallbackPath { get; set; }
    string TenantId { get; set; }
}
