using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Security;

namespace HotelWise.Domain.Dto;

/// <summary>
/// DTO de saída com dados do usuário autenticado no sistema e suas credenciais de token JWT.
/// </summary>
public class GetUserAuthenticatedDto : EntityDtoBase
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GetUserAuthenticatedDto"/> com um token padrão.
    /// </summary>
    public GetUserAuthenticatedDto()
    {
        TokenAuth = new TokenVO();
    }

    /// <summary>
    /// Nome completo ou de exibição do usuário.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Preferência de idioma do usuário (ex: "pt-BR", "en-US").
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Objeto contendo os tokens de acesso e renovação (JWT e refresh token).
    /// </summary>
    public TokenVO? TokenAuth { get; set; }

    /// <summary>
    /// Identificador médico/registro associado ao usuário, se aplicável.
    /// </summary>
    public long? MedicalId { get; set; }
}
