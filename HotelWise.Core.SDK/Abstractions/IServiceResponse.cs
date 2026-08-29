namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada de serviço.
/// </summary>
/// <typeparam name="T">Tipo do payload.</typeparam>
public interface IServiceResponse<T>
{
    T? Data { get; set; }
    bool Success { get; set; }
    string Message { get; set; }
}
