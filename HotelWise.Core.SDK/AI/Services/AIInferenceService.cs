#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;
using SchService = SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Orquestra inferência LLM — herda implementação SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService em SmartCoreHub.Core.SDK.")]
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
