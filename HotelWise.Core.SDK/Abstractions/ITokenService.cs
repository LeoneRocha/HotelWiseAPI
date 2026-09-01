using System.Security.Claims;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de serviço responsável pela emissão, renovação e validação de tokens de autenticação (JWT).
/// Abstrai a geração de access tokens a partir de claims, a emissão de refresh tokens
/// e a recuperação do principal de segurança a partir de um token expirado.
/// </summary>
public interface ITokenService : SmartCoreHub.Core.SDK.Domain.Abstractions.ITokenService
{
}
