namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Informações de versão e ambiente do produto.
/// </summary>
public class AppInformationVersionProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string EnvironmentName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
