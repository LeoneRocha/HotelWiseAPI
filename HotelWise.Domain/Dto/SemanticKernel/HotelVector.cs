using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.Dto.SemanticKernel
{ 
    public class HotelVector
    {
        [VectorStoreRecordKey]
        public ulong HotelId { get; set; }

        [VectorStoreRecordData(IsFilterable = true)]
        public string HotelName { get; set; } = string.Empty;

        [VectorStoreRecordData(IsFullTextSearchable = true)]
        public string Description { get; set; } = string.Empty;

        [VectorStoreRecordVector(Dimensions: 1024)]//BERT-base 768 (Bidirectional Encoder Representations from Transformers) 
        public ReadOnlyMemory<float> DescriptionEmbedding { get; set; }

        [VectorStoreRecordData(IsFilterable = true)]
        public List<string> Tags { get; set; } = new List<string>();
    } 
}
