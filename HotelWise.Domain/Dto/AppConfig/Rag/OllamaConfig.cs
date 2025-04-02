using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// Qdrant service settings.
/// </summary>
public sealed class OllamaConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "OllamaApi";
    public int NumPredict { get; set; } = 500;
    public float Temperature { get; set; } = 0.0f;
    public float TopP { get; set; } = 1.0f;
    public int? Seed { get; set; } = 32;
}