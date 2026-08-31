#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;
using SchService = SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Orquestra inferência LLM — herda implementação SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AIInferenceService : SchService, IAIInferenceService
{
    /// <summary>
    /// Inicializa o serviço com configuração e fábrica SCH.
    /// </summary>
    public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
        : base(configuration, adapterFactory)
    {
    }
}
#endif
