#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using SchAbstractions = SmartCoreHub.Core.SDK.Domain.AI.Abstractions;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter genérico de vector store — casca sobre SCH.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial, implementando <see cref="IDataVector"/>.</typeparam>
public class GenericVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector>
    where TVector : class, IDataVector
{
    private readonly SchAbstractions.IVectorStoreAdapter<TVector> _inner;

    /// <summary>
    /// Casca sobre adapter SCH já construído (via fábrica SCH).
    /// </summary>
    internal GenericVectorStoreAdapter(SchAbstractions.IVectorStoreAdapter<TVector> inner) =>
        _inner = inner;

    /// <summary>
    /// Inicializa uma nova instância construindo o adapter SCH internamente.
    /// </summary>
    public GenericVectorStoreAdapter(
        Serilog.ILogger logger,
        IApplicationIAConfig applicationConfig,
        VectorStore vectorStore,
        Kernel kernel)
    {
        _inner = new SchAdapters.GenericVectorStoreAdapter<TVector>(
            logger,
            applicationConfig,
            vectorStore,
            kernel);
    }

    /// <inheritdoc />
    public Task UpsertDataAsync(string nameCollection, TVector dataVector) =>
        _inner.UpsertDataAsync(nameCollection, dataVector);

    /// <inheritdoc />
    public Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors) =>
        _inner.UpsertDatasAsync(nameCollection, dataVectors);

    /// <inheritdoc />
    public Task<TVector?> GetByKey(string nameCollection, ulong dataKey) =>
        _inner.GetByKey(nameCollection, dataKey);

    /// <inheritdoc />
    public Task<bool> Exists(string nameCollection, ulong dataKey) =>
        _inner.Exists(nameCollection, dataKey);

    /// <inheritdoc />
    public Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SchDto.SearchCriteria searchCriteria) =>
        _inner.VectorizedSearchAsync(nameCollection, searchEmbedding, searchCriteria);

    /// <inheritdoc />
    public Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding) =>
        _inner.SearchAndAnalyzePluginAsync(nameCollection, searchQuery, searchEmbedding);

    /// <inheritdoc />
    public Task DeleteAsync(string nameCollection, long dataKey) =>
        _inner.DeleteAsync(nameCollection, dataKey);
}
#endif
