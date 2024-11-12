using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;

namespace HotelWise.Service.AI
{
    public class HotelVectorStoreService : IVectorStoreService<HotelVector>
    {
        private readonly IVectorStoreAdapter<HotelVector> _adapter;
        private readonly IAIInferenceService _aIInferenceService;
        private const string nameCollection = "skhotels";

        public HotelVectorStoreService(IVectorStoreAdapterFactory adapterFactory, IAIInferenceService aIInferenceService)
        {
            _adapter = adapterFactory.CreateAdapter<HotelVector>();
            _aIInferenceService = aIInferenceService;
        }

        public async Task<float[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, EIAInferenceAdapterType.Mistral);
        }

        public async Task<HotelVector?> GetById(long dataKey)
        {
            var hotelVector = await _adapter.GetByKey(nameCollection, (ulong)dataKey);

            if (hotelVector != null)
            {
                return hotelVector;
            }
            return null;
        }

        public async Task UpsertDataAsync(HotelVector hotelVector)
        {
            var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotelVector.Description, EIAInferenceAdapterType.Mistral);

            hotelVector.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);            

            await _adapter.UpsertDataAsync(nameCollection, hotelVector);
        }

        public async Task UpsertDatasAsync(HotelVector[] hotels)
        {
            var hotelVectors = new List<HotelVector>();

            foreach (HotelVector hotel in hotels)
            {
                if (!await _adapter.Exists(nameCollection, hotel.DataKey))
                {
                    var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, EIAInferenceAdapterType.Mistral);

                    hotel.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);
                }
            }
            if (hotelVectors.Count > 0)
            {
                await _adapter.UpsertDatasAsync(nameCollection, hotelVectors.ToArray());
            }
        }
        public async Task<HotelVector[]> SearchDatasAsync(string searchText)
        {
            //Get semantic search 
            var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchText, EIAInferenceAdapterType.Mistral);

            var hotelsVector = await _adapter.SearchDatasAsync(nameCollection, embeddingSearchText);
             
            return hotelsVector ;
        }
    }
}