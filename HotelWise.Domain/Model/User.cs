using HotelWise.Core.SDK.Domain;

namespace HotelWise.Domain.Model;

/// <summary>
/// Entidade de domínio que representa um usuário do sistema, com credenciais, controle de acesso e dados de sessão.
/// </summary>
public class User : EntityBaseWithNameEmail
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="User"/>.
    /// </summary>
    public User()
    { 
    }

    #region Columns 

    /// <summary>
    /// Nome de login único do usuário para acesso ao sistema.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Hash criptográfico da senha do usuário (HMACSHA512).
    /// </summary>
    public byte[] PasswordHash { get; set; } = [];

    /// <summary>
    /// Chave de salt utilizada na criptografia da senha.
    /// </summary>
    public byte[] PasswordSalt { get; set; } = [];

    /// <summary>
    /// Perfil ou papel de autorização do usuário (ex: "Admin", "Manager", "User").
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o usuário possui privilégios de administrador do sistema.
    /// </summary>
    public bool Admin { get; set; }

    /// <summary>
    /// Preferência de idioma do usuário (ex: "pt-BR", "en-US").
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do fuso horário preferido do usuário (ex: "E. South America Standard Time").
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Token opaco utilizado para renovação do access token JWT.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora limite de expiração do refresh token.
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }

    #endregion Columns 
}
