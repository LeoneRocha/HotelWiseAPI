using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;
/// <summary>
/// Weaviate service settings.
/// </summary>
public sealed class WeaviateConfig
{
    public const string ConfigSectionName = "Weaviate";

    [Required]
    public string Endpoint { get; set; } = string.Empty;
}