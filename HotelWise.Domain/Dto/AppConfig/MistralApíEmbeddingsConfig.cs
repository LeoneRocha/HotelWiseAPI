using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;
 
public sealed class MistralApíEmbeddingsConfig
{
    public const string ConfigSectionName = "MistralApíEmbeddings";

    [Required]
    public string ModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string? OrgId { get; set; } = null;
}
