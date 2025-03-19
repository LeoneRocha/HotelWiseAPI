using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

public sealed class MistralApiEmbeddingsConfig : AiInferenceConfigBase
{    
    public new static string ConfigSectionName => "MistralApiEmbeddings";
     
}
