using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Service.AI
{
    public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
    {
        private readonly IApplicationConfig _applicationConfig;
         
        public VectorStoreAdapterFactory(IApplicationConfig applicationConfig)
        {
            _applicationConfig = applicationConfig;   
        }
        public IVectorStoreAdapter CreateAdapter()
        {
            return new SemanticKernelVectorStoreAdapter(_applicationConfig);
        }
    }
}