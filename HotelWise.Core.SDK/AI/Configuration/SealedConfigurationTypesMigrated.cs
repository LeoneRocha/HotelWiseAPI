using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Tipos sealed neste namespace migraram para
/// <see cref="SmartCoreHub.Core.SDK.Domain.AI.Configuration.RagConfig"/> e
/// <see cref="SmartCoreHub.Core.SDK.Domain.AI.Configuration.ApplicationIAConfig"/>.
/// Use os FQNs SCH diretamente (ou global usings do projeto).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration", targetPackage: "SmartCoreHub.Core.SDK", description: "Tipos sealed migrados para SmartCoreHub.Core.SDK.Domain.AI.Configuration.")]
public static class SealedConfigurationTypesMigrated
{
    /// <summary>FQN SCH de <c>RagConfig</c>.</summary>
    public const string RagConfig = "SmartCoreHub.Core.SDK.Domain.AI.Configuration.RagConfig";

    /// <summary>FQN SCH de <c>ApplicationIAConfig</c>.</summary>
    public const string ApplicationIAConfig = "SmartCoreHub.Core.SDK.Domain.AI.Configuration.ApplicationIAConfig";
}
