using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure AI Search service settings.
/// </summary>
public sealed class AzureAISearchConfig
{
    public const string ConfigSectionName = "AzureAISearch";

    [Required]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}