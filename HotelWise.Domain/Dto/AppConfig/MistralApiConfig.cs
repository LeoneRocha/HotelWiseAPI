using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Qdrant service settings.
/// </summary>
public sealed class MistralApiConfig : AiInferenceConfigBase
{ 
    public new static string ConfigSectionName => "MistralApí"; 
} 