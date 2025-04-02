using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// Azure OpenAI Embeddings service settings.
/// </summary>
public sealed class AzureOpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "AzureOpenAIEmbeddings";

    [Required]
    public string DeploymentName { get; set; } = string.Empty;

}