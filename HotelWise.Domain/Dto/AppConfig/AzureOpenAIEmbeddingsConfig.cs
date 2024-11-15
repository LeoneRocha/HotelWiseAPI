using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure OpenAI Embeddings service settings.
/// </summary>
public sealed class AzureOpenAIEmbeddingsConfig
{
    public const string ConfigSectionName = "AzureOpenAIEmbeddings";

    [Required]
    public string DeploymentName { get; set; } = string.Empty;

    [Required]
    public string Endpoint { get; set; } = string.Empty;
}