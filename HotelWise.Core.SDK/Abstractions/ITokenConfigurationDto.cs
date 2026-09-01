
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de configuração utilizada na emissão e validação de tokens JWT.
/// Define audiência, emissor, segredo de assinatura e prazos de validade
/// consumidos pelos serviços de autenticação do SDK.
/// </summary>
public interface ITokenConfigurationDto : SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenConfigurationDto
{
}
