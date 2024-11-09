﻿using Microsoft.Extensions.VectorData;

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

        [VectorStoreRecordVector(Dimensions: 4, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
        public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }

        [VectorStoreRecordData(IsFilterable = true)]
        public string[] Tags { get; set; } = [];
    }
}