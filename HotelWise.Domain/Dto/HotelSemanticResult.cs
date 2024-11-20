using HotelWise.Domain.Model;

namespace HotelWise.Domain.Dto
{
    public class HotelSemanticResult
    {
        public string PromptResultContent { get; set; } = string.Empty;
        public HotelDto[] HotelsVectorResult { get; set; } = [];

        public HotelDto[] HotelsIAResult { get; set; } = [];
    }
}
