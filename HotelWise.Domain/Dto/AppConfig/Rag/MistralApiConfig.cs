using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// Qdrant service settings.
/// </summary>
public sealed class MistralApiConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "MistralApi";
}