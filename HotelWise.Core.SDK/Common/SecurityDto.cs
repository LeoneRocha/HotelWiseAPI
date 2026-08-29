namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com dados de segurança usados na geração e contextualização de tokens.
/// Transporta identidade, papel e chave de configuração associada ao usuário autenticado.
/// </summary>
public class SecurityDto
{
    /// <summary>
    /// Nome do usuário ou principal de segurança.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Papel (role) associado ao principal de segurança.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do usuário ou principal de segurança.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Chave ou referência de configuração de segurança usada na emissão do token.
    /// </summary>
    public string SecurityKeyConfig { get; set; } = string.Empty;
}
