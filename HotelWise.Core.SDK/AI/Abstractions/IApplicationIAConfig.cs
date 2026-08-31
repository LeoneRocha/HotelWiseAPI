namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato agregado de configuração de IA da aplicação.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IApplicationIAConfig
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig
{
}
