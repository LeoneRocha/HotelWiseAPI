#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using HotelWise.Core.SDK.AI.Abstractions;
using Microsoft.Extensions.VectorData;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Base de dado vetorial com atributos de vector store.
/// </summary>
public abstract class DataVectorBase : IDataVector
{
    [VectorStoreKey]
    public ulong DataKey { get; set; } = ulong.MinValue;

    [VectorStoreVector(1024)]
    public virtual ReadOnlyMemory<float> Embedding { get; set; } = new ReadOnlyMemory<float>();

    [NotMapped]
    [XmlIgnore]
    [JsonIgnore]
    public double Score { get; set; }

    [VectorStoreData(IsIndexed = true)]
    public List<string> Tags { get; set; } = new List<string>();
}
#endif
