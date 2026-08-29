using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Domain.Interfaces.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IVectorStoreAdapterFactory.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IVectorStoreAdapterFactory
    {
        IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : class, IDataVector;
    }
}
