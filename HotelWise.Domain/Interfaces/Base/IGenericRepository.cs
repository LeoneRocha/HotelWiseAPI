namespace HotelWise.Domain.Interfaces.Entity
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Namespace aninhado duplicado preservado no host para compatibilidade com consumidores existentes.
    /// </summary>
    namespace HotelWise.Domain.Interfaces.Entity
    {
        [Obsolete(
            "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Abstractions.IGenericRepository<T>.",
            error: false,
            DiagnosticId = "HW_CORE_SDK_REPO")]
        public interface IGenericRepository<T> : global::HotelWise.Core.SDK.Abstractions.IGenericRepository<T>
            where T : class
        {
        }
    }
}
