namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de configuração utilizada na emissão e validação de tokens JWT.
/// Define audiência, emissor, segredo de assinatura e prazos de validade
/// consumidos pelos serviços de autenticação do SDK.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface ITokenConfigurationDto : SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto
{
}
