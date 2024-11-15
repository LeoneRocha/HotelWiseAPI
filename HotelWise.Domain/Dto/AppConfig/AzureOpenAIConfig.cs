using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure OpenAI service settings.
/// </summary>
public sealed class AzureOpenAIConfig
{
    public const string ConfigSectionName = "AzureOpenAI";

    [Required]
    public string ChatDeploymentName { get; set; } = string.Empty;

    [Required]
    public string Endpoint { get; set; } = string.Empty;
}