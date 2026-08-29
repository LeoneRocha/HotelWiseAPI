namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de fuso horário.
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de configuração.
/// </summary>
public class TimeZoneDisplayDto
{
    /// <summary>
    /// Identificador do fuso horário (ex.: ID do sistema ou IANA).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome de exibição do fuso horário.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
