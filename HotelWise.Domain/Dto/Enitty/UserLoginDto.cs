namespace HotelWise.Domain.Dto.Enitty;

/// <summary>
/// DTO de entrada para autenticação de usuário contendo credenciais de login e senha.
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// Identificador de login do usuário.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Senha em texto simples para validação contra o hash persistido.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
