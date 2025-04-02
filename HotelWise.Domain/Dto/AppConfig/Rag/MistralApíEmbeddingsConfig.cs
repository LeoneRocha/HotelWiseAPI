using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig.Rag;

public sealed class MistralApiEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "MistralApiEmbeddings";

}
