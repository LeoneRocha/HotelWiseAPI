using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig.Rag;
/// <summary>
/// OpenAI Embeddings service settings.
/// </summary>
public sealed class OpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "OpenAIEmbeddings";
}