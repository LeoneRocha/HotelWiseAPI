
namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada — casca sobre SCH.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
public interface IServiceResponse<T> : SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse<T>
{
}
