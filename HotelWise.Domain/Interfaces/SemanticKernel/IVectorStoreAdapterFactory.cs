using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IVectorStoreAdapterFactory
    {
        IVectorStoreAdapter<TVector> CreateAdapter<TVector>();
    }
}