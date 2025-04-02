using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Dto.IA.SemanticKernel
{
    public class HotelSemanticResult
    {
        public string PromptResultContent { get; set; } = string.Empty;
        public HotelDto[] HotelsVectorResult { get; set; } = [];

        public HotelDto[] HotelsIAResult { get; set; } = [];
    }
}
