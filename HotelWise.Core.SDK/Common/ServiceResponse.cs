
namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Resposta padronizada de operações de serviço.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
public class ServiceResponse<T> : SmartCoreHub.Core.SDK.Common.ServiceResponse<T>, HotelWise.Core.SDK.Abstractions.IServiceResponse<T>
{
}
