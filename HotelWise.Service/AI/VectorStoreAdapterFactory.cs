using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Service.AI
{
    public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
    {
        private readonly IVectorStoreSettingsDto _vectorStoreSettingsDto;
         
        public VectorStoreAdapterFactory(IVectorStoreSettingsDto vectorStoreSettingsDto)
        {
                _vectorStoreSettingsDto = vectorStoreSettingsDto;   
        }
        public IVectorStoreAdapter CreateAdapter()
        {
            return new SemanticKernelVectorStoreAdapter(_vectorStoreSettingsDto);
        }
    }
}