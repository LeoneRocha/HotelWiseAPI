#if NET8_0_OR_GREATER
using AutoMapper;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Base mínima para serviços de vector store tipados por entidade.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.GenericVectorStoreServiceBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class GenericVectorStoreServiceBase : SmartCoreHub.Core.SDK.Service.AI.Services.GenericVectorStoreServiceBase
{
    protected GenericVectorStoreServiceBase(IMapper mapper, Serilog.ILogger logger)
        : base(mapper, logger)
    {
    }
}
#endif
