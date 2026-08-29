#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de vector store.
/// </summary>
public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
{
    private readonly IApplicationIAConfig _applicationConfig;
    private readonly VectorStore _vectorStore;
    private readonly Kernel _kernel;
    private readonly Serilog.ILogger _logger;

    public VectorStoreAdapterFactory(
        IApplicationIAConfig applicationConfig,
        VectorStore vectorStore,
        Kernel kernel,
        Serilog.ILogger logger)
    {
        _applicationConfig = applicationConfig;
        _vectorStore = vectorStore;
        _kernel = kernel;
        _logger = logger;
    }

    public IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector
    {
        return new GenericVectorStoreAdapter<TVector>(_logger, _applicationConfig, _vectorStore, _kernel);
    }
}
#endif
