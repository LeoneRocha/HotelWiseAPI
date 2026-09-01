#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using HotelWise.Core.SDK.AI.DTO;
using SchFactory = SmartCoreHub.Core.SDK.Service.AI.Services.VectorStoreAdapterFactory;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de vector store — herda SCH; retorna casca HW nos adapters.
/// </summary>
public class VectorStoreAdapterFactory : SchFactory, IVectorStoreAdapterFactory
{
    /// <summary>
    /// Inicializa a fábrica delegando configuração SCH.
    /// </summary>
    public VectorStoreAdapterFactory(
        IApplicationIAConfig applicationConfig,
        Microsoft.Extensions.VectorData.VectorStore vectorStore,
        Microsoft.SemanticKernel.Kernel kernel,
        Serilog.ILogger logger)
        : base(applicationConfig, vectorStore, kernel, logger)
    {
    }

    /// <inheritdoc />
    public new IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector =>
        new GenericVectorStoreAdapter<TVector>(base.CreateAdapter<TVector>());
}
#endif
