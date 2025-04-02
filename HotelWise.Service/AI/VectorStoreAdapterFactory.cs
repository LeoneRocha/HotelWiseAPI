using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Interfaces.AppConfig;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;

namespace HotelWise.Service.AI
{
    public class VectorStoreAdapterFactory : IVectorStoreAdapterFactory
    {
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IVectorStore _vectorStore;
        private readonly Kernel _kernel;
        private readonly Serilog.ILogger _logger;
        public VectorStoreAdapterFactory(IApplicationIAConfig applicationConfig, IVectorStore vectorStore, Kernel kernel, Serilog.ILogger logger)
        {
            _applicationConfig = applicationConfig;
            _vectorStore = vectorStore;
            _kernel = kernel;
            _logger = logger;
        }
        public IVectorStoreAdapter<TVector> CreateAdapter<TVector>() where TVector : IDataVector
        {
            return new GenericVectorStoreAdapter<TVector>(_logger, _applicationConfig, _vectorStore, _kernel);
        }
    }
}