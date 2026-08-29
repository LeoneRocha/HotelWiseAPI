#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using HotelWise.Core.SDK.AI.Abstractions;
using Microsoft.Extensions.VectorData;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Base de dado vetorial com atributos de vector store (chave, embedding, tags).
/// Implementa <see cref="IDataVector"/> para registros indexáveis no pipeline RAG
/// via Microsoft.Extensions.VectorData.
/// </summary>
public abstract class DataVectorBase : IDataVector
{
    /// <summary>
    /// Chave única do registro no vector store (<see cref="VectorStoreKeyAttribute"/>).
    /// </summary>
    [VectorStoreKey]
    public ulong DataKey { get; set; } = ulong.MinValue;

    /// <summary>
    /// Vetor de embedding do registro (dimensão padrão 1024).
    /// </summary>
    [VectorStoreVector(1024)]
    public virtual ReadOnlyMemory<float> Embedding { get; set; } = new ReadOnlyMemory<float>();

    /// <summary>
    /// Score de similaridade preenchido após busca vetorial (não persistido).
    /// </summary>
    [NotMapped]
    [XmlIgnore]
    [JsonIgnore]
    public double Score { get; set; }

    /// <summary>
    /// Tags indexáveis para filtros na busca semântica.
    /// </summary>
    [VectorStoreData(IsIndexed = true)]
    public List<string> Tags { get; set; } = new List<string>();
}
#endif
