using HotelWise.Core.SDK.Abstractions;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Resposta padronizada de operações de serviço.
/// Implementa <see cref="IServiceResponse{T}"/> e estende o contrato com lista de erros
/// e indicador de não autorização, para comunicação uniforme com a API e o cliente.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.ServiceResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class ServiceResponse<T> : IServiceResponse<T>
{
    /// <summary>
    /// Payload de dados da operação; pode ser <c>null</c> em falhas ou operações sem corpo.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Indica se a operação foi concluída com sucesso. Valor padrão: <c>true</c>.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Mensagem descritiva do resultado (sucesso, aviso ou erro).
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Lista de erros detalhados associados à resposta (validações, falhas de negócio, etc.).
    /// </summary>
    public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();

    /// <summary>
    /// Indica se a operação falhou por falta de autorização do usuário.
    /// </summary>
    public bool Unauthorized { get; set; }
}
