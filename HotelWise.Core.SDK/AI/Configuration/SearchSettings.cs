namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configurações auxiliares de busca vetorial / RAG.
/// Controla atrasos e comportamentos relacionados à consulta no vector store.
/// </summary>
public class SearchSettings
{
    /// <summary>
    /// Atraso em milissegundos antes de executar a busca (ex.: após indexação).
    /// </summary>
    public int DelayBeforeSearchMilliseconds { get; set; }
}
