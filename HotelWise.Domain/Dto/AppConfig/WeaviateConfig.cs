namespace HotelWise.Domain.Dto.AppConfig;
/// <summary>
/// Weaviate service settings.
/// </summary>
public sealed class WeaviateConfig : AiInferenceConfigBase
{ 
    public new static string ConfigSectionName => "Weaviate"; 
}