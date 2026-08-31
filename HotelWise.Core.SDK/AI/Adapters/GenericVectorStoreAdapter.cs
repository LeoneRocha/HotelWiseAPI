#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter genérico de vector store — casca sobre SCH.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial, implementando <see cref="IDataVector"/>.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.GenericVectorStoreAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GenericVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector>
    where TVector : class, IDataVector
{
    private readonly SchAdapters.GenericVectorStoreAdapter<TVector> _inner;

    public GenericVectorStoreAdapter(
        Serilog.ILogger logger,
        IApplicationIAConfig applicationConfig,
        VectorStore vectorStore,
        Kernel kernel)
    {
        _inner = new SchAdapters.GenericVectorStoreAdapter<TVector>(
            logger,
            ApplicationIAConfigSchBridge.ToSch(applicationConfig),
            vectorStore,
            kernel);
    }

    public Task UpsertDataAsync(string nameCollection, TVector dataVector) =>
        _inner.UpsertDataAsync(nameCollection, dataVector);

    public Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors) =>
        _inner.UpsertDatasAsync(nameCollection, dataVectors);

    public Task<TVector?> GetByKey(string nameCollection, ulong dataKey) =>
        _inner.GetByKey(nameCollection, dataKey);

    public Task<bool> Exists(string nameCollection, ulong dataKey) =>
        _inner.Exists(nameCollection, dataKey);

    public Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria) =>
        _inner.VectorizedSearchAsync(nameCollection, searchEmbedding, searchCriteria);

    public Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding) =>
        _inner.SearchAndAnalyzePluginAsync(nameCollection, searchQuery, searchEmbedding);

    public Task DeleteAsync(string nameCollection, long dataKey) =>
        _inner.DeleteAsync(nameCollection, dataKey);
}
#endif
