using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.Dto.IA.SemanticKernel
{
    public class HotelVector : HotelWise.Core.SDK.AI.DTO.DataVectorBase
    {
        [VectorStoreData(IsIndexed = true)]
        public string HotelName { get; set; } = string.Empty;

        [VectorStoreData(IsFullTextIndexed = true)]
        public string Description { get; set; } = string.Empty;
    }
}
