using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// Azure OpenAI service settings.
/// </summary>
public sealed class AzureOpenAIConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "AzureOpenAI";

    [Required]
    public string ChatDeploymentName { get; set; } = string.Empty;

}