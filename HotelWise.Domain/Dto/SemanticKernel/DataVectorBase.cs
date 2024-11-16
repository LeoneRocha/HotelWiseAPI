using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.VectorData;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HotelWise.Domain.Dto.SemanticKernel
{
    public abstract class DataVectorBase : IDataVector
    {
        [VectorStoreRecordKey]
        public ulong DataKey { get; set; } = ulong.MinValue;
         
        [VectorStoreRecordVector(Dimensions: 1024)]//BERT-base 768 (Bidirectional Encoder Representations from Transformers) 
        public virtual ReadOnlyMemory<float> Embedding { get; set; } = new ReadOnlyMemory<float>();

        [NotMapped]
        [XmlIgnore]
        [JsonIgnore]
        public double Score { get; set; } = 0d;
    } 
}
