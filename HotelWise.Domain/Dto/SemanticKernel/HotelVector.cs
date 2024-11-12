using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.Dto.SemanticKernel
{
    public class HotelVector : DataVectorBase
    {
        [VectorStoreRecordData(IsFilterable = true)]
        public string HotelName { get; set; } = string.Empty;

        [VectorStoreRecordData(IsFullTextSearchable = true)]
        public string Description { get; set; } = string.Empty;
          
        [VectorStoreRecordData(IsFilterable = true)]
        public List<string> Tags { get; set; } = new List<string>(); 
    }
}
