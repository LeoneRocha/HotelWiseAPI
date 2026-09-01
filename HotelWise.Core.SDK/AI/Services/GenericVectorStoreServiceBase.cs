#if NET8_0_OR_GREATER
using AutoMapper;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Base mínima para serviços de vector store tipados por entidade.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.AI.Services.GenericVectorStoreServiceBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.AI.Services.GenericVectorStoreServiceBase em SmartCoreHub.Core.SDK.")]
public abstract class GenericVectorStoreServiceBase : SmartCoreHub.Core.SDK.Service.AI.Services.GenericVectorStoreServiceBase
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GenericVectorStoreServiceBase"/>.
    /// </summary>
    /// <param name="mapper">Instância do AutoMapper.</param>
    /// <param name="logger">Logger Serilog.</param>
    protected GenericVectorStoreServiceBase(IMapper mapper, Serilog.ILogger logger)
        : base(mapper, logger)
    {
    }
}
#endif
