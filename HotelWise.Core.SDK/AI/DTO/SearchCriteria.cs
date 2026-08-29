namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por <see cref="Abstractions.IVectorStoreAdapter{TVector}.VectorizedSearchAsync"/>
/// e serviços de vector store tipados.
/// </summary>
public class SearchCriteria
{
    /// <summary>
    /// Quantidade máxima de registros a recuperar na busca.
    /// </summary>
    public int MaxHotelRetrieve { get; set; }

    /// <summary>
    /// Texto da consulta semântica.
    /// </summary>
    public string SearchTextCriteria { get; set; } = string.Empty;

    /// <summary>
    /// Tags usadas para filtrar resultados no vector store.
    /// </summary>
    public string[] TagsCriteria { get; set; } = Array.Empty<string>();
}
