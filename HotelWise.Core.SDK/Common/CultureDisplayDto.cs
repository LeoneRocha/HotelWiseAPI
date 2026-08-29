namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de cultura (idioma/região).
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de localização.
/// </summary>
public class CultureDisplayDto
{
    /// <summary>
    /// Identificador da cultura (ex.: código BCP 47 como <c>pt-BR</c>).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome de exibição da cultura.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
