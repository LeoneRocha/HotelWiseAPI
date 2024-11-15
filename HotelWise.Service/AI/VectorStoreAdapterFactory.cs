using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;

namespace HotelWise.Service.AI
{
    public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
    {
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IVectorStore _vectorStore;
        public VectorStoreAdapterFactory(IApplicationIAConfig applicationConfig, IVectorStore vectorStore)
        {
            _applicationConfig = applicationConfig;
            _vectorStore = vectorStore;
        }
        public IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : IDataVector
        {
            return new GenericVectorStoreAdapter<TVector>(_applicationConfig, _vectorStore);
        }
    }
}