namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de dado vetorial genérico usado no vector store e no pipeline RAG.
/// Representa um registro indexável com chave, embedding, score de similaridade e tags.
/// </summary>
public interface IDataVector
{
    /// <summary>
    /// Chave única do registro no vector store.
    /// </summary>
    ulong DataKey { get; set; }

    /// <summary>
    /// Vetor de embedding associado ao registro.
    /// </summary>
    ReadOnlyMemory<float> Embedding { get; set; }

    /// <summary>
    /// Score de similaridade retornado pela busca vetorial (quando disponível).
    /// </summary>
    double Score { get; set; }

    /// <summary>
    /// Tags indexáveis usadas para filtrar resultados na busca semântica.
    /// </summary>
    List<string> Tags { get; set; }
}
