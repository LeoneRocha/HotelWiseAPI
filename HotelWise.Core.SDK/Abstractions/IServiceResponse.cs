namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada de operações de serviço.
/// Encapsula o payload tipado, o indicador de sucesso e uma mensagem descritiva
/// para comunicação uniforme entre camadas e com a API.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
public interface IServiceResponse<T>
{
    /// <summary>
    /// Payload de dados da operação; pode ser <c>null</c> em falhas ou operações sem corpo.
    /// </summary>
    T? Data { get; set; }

    /// <summary>
    /// Indica se a operação foi concluída com sucesso.
    /// </summary>
    bool Success { get; set; }

    /// <summary>
    /// Mensagem descritiva do resultado (sucesso, aviso ou erro).
    /// </summary>
    string Message { get; set; }
}
