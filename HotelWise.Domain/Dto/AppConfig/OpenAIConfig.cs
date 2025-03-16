using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// OpenAI service settings.
/// </summary>
public sealed class OpenAIConfig :  AiInferenceConfigBase
{ 
    public new static string ConfigSectionName => "OpenAI"; 
}