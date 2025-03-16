using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

public sealed class MistralApíEmbeddingsConfig : AiInferenceConfigBase
{    
    public new static string ConfigSectionName => "MistralApíEmbeddings";
     
}
