using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Service.AI
{
    public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
    {
        public IVectorStoreAdapter CreateAdapter()
        {
            return new SemanticKernelVectorStoreAdapter();
        }
    }
}