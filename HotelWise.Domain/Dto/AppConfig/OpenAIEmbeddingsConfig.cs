using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;
/// <summary>
/// OpenAI Embeddings service settings.
/// </summary>
public sealed class OpenAIEmbeddingsConfig : AiInferenceConfigBase
{ 
    public new static string ConfigSectionName => "OpenAIEmbeddings"; 
}