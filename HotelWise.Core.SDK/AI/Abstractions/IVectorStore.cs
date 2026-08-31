using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de vector store genérico para operações CRUD e busca semântica
/// sobre documentos tipados por <typeparamref name="TVector"/> no pipeline RAG.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial, implementando <see cref="IDataVector"/>.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapter<TVector> where TVector : class, IDataVector
{
    /// <summary>
    /// Insere ou atualiza um único registro vetorial na coleção informada.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="dataVector">Registro vetorial a persistir.</param>
    /// <returns>Tarefa que conclui quando o upsert for finalizado.</returns>
    Task UpsertDataAsync(string nameCollection, TVector dataVector);

    /// <summary>
    /// Insere ou atualiza múltiplos registros vetoriais na coleção informada.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="dataVectors">Registros vetoriais a persistir.</param>
    /// <returns>Tarefa que conclui quando todos os upserts forem finalizados.</returns>
    Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors);

    /// <summary>
    /// Obtém um registro vetorial pela chave na coleção informada.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="dataKey">Chave do registro.</param>
    /// <returns>O registro encontrado, ou <c>null</c> se não existir.</returns>
    Task<TVector?> GetByKey(string nameCollection, ulong dataKey);

    /// <summary>
    /// Verifica se um registro com a chave informada existe na coleção.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="dataKey">Chave do registro.</param>
    /// <returns><c>true</c> se o registro existir; caso contrário, <c>false</c>.</returns>
    Task<bool> Exists(string nameCollection, ulong dataKey);

    /// <summary>
    /// Executa busca vetorial por similaridade com embedding e critérios opcionais (ex.: tags).
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="searchEmbedding">Vetor de embedding da consulta.</param>
    /// <param name="searchCriteria">Critérios de busca (limite, texto, tags).</param>
    /// <returns>Registros mais similares, com <see cref="SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector.Score"/> preenchido quando disponível.</returns>
    Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria);

    /// <summary>
    /// Busca vetorial combinada com análise via plugin do Semantic Kernel (RAG assistido por LLM).
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="searchQuery">Consulta textual do usuário.</param>
    /// <param name="searchEmbedding">Vetor de embedding da consulta.</param>
    /// <returns>Registros resultantes da busca e análise.</returns>
    Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding);

    /// <summary>
    /// Remove um registro vetorial da coleção pela chave.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção no vector store.</param>
    /// <param name="dataKey">Chave do registro a remover.</param>
    /// <returns>Tarefa que conclui quando a exclusão for finalizada.</returns>
    Task DeleteAsync(string nameCollection, long dataKey);
}

/// <summary>
/// Fábrica de adapters de vector store tipados por <see cref="IDataVector"/>.
/// Usada para obter instâncias de <see cref="IVectorStoreAdapter{TVector}"/> no DI.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreAdapterFactory
{
    /// <summary>
    /// Cria um adapter de vector store para o tipo de vetor informado.
    /// </summary>
    /// <typeparam name="TVector">Tipo do registro vetorial.</typeparam>
    /// <returns>Instância de <see cref="IVectorStoreAdapter{TVector}"/>.</returns>
    IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector;
}

/// <summary>
/// Serviço de vector store tipado por entidade de domínio.
/// Abstrai upsert, busca semântica, geração de embedding e exclusão no fluxo RAG.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade de domínio mapeada para o vector store.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IVectorStoreService<TEntity>
{
    /// <summary>
    /// Insere ou atualiza a entidade no vector store (com embedding quando aplicável).
    /// </summary>
    /// <param name="entity">Entidade a persistir.</param>
    /// <returns>Tarefa que conclui quando o upsert for finalizado.</returns>
    Task UpsertDataAsync(TEntity entity);

    /// <summary>
    /// Insere ou atualiza múltiplas entidades no vector store.
    /// </summary>
    /// <param name="listEntity">Entidades a persistir.</param>
    /// <returns>Tarefa que conclui quando todos os upserts forem finalizados.</returns>
    Task UpsertDatasAsync(TEntity[] listEntity);

    /// <summary>
    /// Obtém uma entidade pela chave numérica.
    /// </summary>
    /// <param name="dataKey">Identificador da entidade no store.</param>
    /// <returns>A entidade encontrada, ou <c>null</c> se não existir.</returns>
    Task<TEntity?> GetById(long dataKey);

    /// <summary>
    /// Executa busca vetorial/semântica conforme critérios e retorna resposta de serviço.
    /// </summary>
    /// <param name="searchCriteria">Critérios de busca (texto, tags, limite).</param>
    /// <returns>Resposta de serviço contendo as entidades encontradas.</returns>
    Task<ServiceResponse<TEntity[]>> VectorizedSearchAsync(SearchCriteria searchCriteria);

    /// <summary>
    /// Busca e analisa resultados via plugin LLM a partir do texto informado.
    /// </summary>
    /// <param name="searchText">Texto da consulta do usuário.</param>
    /// <returns>Resposta de serviço contendo as entidades analisadas.</returns>
    Task<ServiceResponse<TEntity[]>> SearchAndAnalyzePluginAsync(string searchText);

    /// <summary>
    /// Gera o embedding vetorial do texto informado.
    /// </summary>
    /// <param name="text">Texto a vetorizar.</param>
    /// <returns>Array de floats do embedding, ou <c>null</c> se não for possível gerar.</returns>
    Task<float[]?> GenerateEmbeddingAsync(string text);

    /// <summary>
    /// Remove a entidade do vector store pela chave.
    /// </summary>
    /// <param name="dataKey">Identificador da entidade a remover.</param>
    /// <returns>Tarefa que conclui quando a exclusão for finalizada.</returns>
    Task DeleteAsync(long dataKey);
}
