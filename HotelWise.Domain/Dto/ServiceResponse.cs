using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Dto
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Shim em cópia (sem herança) para preservar <c>List&lt;ErrorResponse&gt;</c> do host
    /// sem quebrar invariância de List&lt;T&gt; durante a migração.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.ServiceResponse<T>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
        public bool Unauthorized { get; set; }
    }
}
