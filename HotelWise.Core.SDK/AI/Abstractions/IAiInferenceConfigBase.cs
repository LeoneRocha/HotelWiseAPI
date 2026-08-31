namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato base de configuração de inferência IA.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAiInferenceConfigBase : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase
{
}
