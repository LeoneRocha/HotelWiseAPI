#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de vector store tipados por <see cref="IDataVector"/>.
/// Implementa <see cref="IVectorStoreAdapterFactory"/> criando
/// <see cref="GenericVectorStoreAdapter{TVector}"/> com logger, config, store e kernel.
/// </summary>
/// <example>
/// <code>
/// // Registro DI
/// services.AddScoped&lt;IVectorStoreAdapterFactory, VectorStoreAdapterFactory&gt;();
///
/// // Uso
/// var adapter = factory.CreateAdapter&lt;HotelVector&gt;();
/// await adapter.UpsertDataAsync("hotels", vector);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.VectorStoreAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
{
    /// <summary>
    /// Configuração agregada de IA.
    /// </summary>
    private readonly IApplicationIAConfig _applicationConfig;

    /// <summary>
    /// Vector store injetado.
    /// </summary>
    private readonly VectorStore _vectorStore;

    /// <summary>
    /// Kernel do Semantic Kernel.
    /// </summary>
    private readonly Kernel _kernel;

    /// <summary>
    /// Logger estruturado.
    /// </summary>
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Inicializa a fábrica com dependências necessárias aos adapters genéricos.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA.</param>
    /// <param name="vectorStore">Vector store registrado no DI.</param>
    /// <param name="kernel">Kernel do Semantic Kernel.</param>
    /// <param name="logger">Logger Serilog.</param>
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

    /// <summary>
    /// Cria um <see cref="GenericVectorStoreAdapter{TVector}"/> para o tipo informado.
    /// </summary>
    /// <typeparam name="TVector">Tipo do registro vetorial.</typeparam>
    /// <returns>Adapter tipado de vector store.</returns>
    /// <example>
    /// <code>
    /// IVectorStoreAdapter&lt;HotelVector&gt; adapter = factory.CreateAdapter&lt;HotelVector&gt;();
    /// </code>
    /// </example>
    public IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector
    {
        return new GenericVectorStoreAdapter<TVector>(_logger, _applicationConfig, _vectorStore, _kernel);
    }
}
#endif
