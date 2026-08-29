namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração geral da aplicação.
/// Agrupa content-types HTTP, formatos de data/hora e mensagens de configuração inválida.
/// </summary>
public static class AppConfigConstants
{
    /// <summary>
    /// Content-Type HTTP para JSON (<c>application/json</c>).
    /// </summary>
    public const string ApplicationContentJon = "application/json";

    /// <summary>
    /// Formato de data/hora padrão: <c>yyyy-MM-dd HH:mm:ss</c>.
    /// </summary>
    public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Formato de data/hora ISO com sufixo Z: <c>yyyy-MM-ddTHH:mm:ssZ</c>.
    /// </summary>
    public const string DATE_FORMAT2 = "yyyy-MM-ddTHH:mm:ssZ";

    /// <summary>
    /// Mensagem de erro quando a configuração da aplicação é nula.
    /// </summary>
    public const string ConfigurationConfigurationNotBeNull = "Configuration cannot be null.";
}
