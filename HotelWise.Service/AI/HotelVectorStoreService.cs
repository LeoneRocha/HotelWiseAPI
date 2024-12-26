using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;

namespace HotelWise.Service.AI
{
    public class HotelVectorStoreService : IVectorStoreService<HotelVector>
    {
        private readonly IVectorStoreAdapter<HotelVector> _adapter;
        private readonly IAIInferenceService _aIInferenceService;
        private const string nameCollection = "skhotels";
        private readonly IAInferenceAdapterType _eIAInferenceAdapterType;

        public HotelVectorStoreService(IVectorStoreAdapterFactory adapterFactory, IAIInferenceService aIInferenceService)
        {
            _eIAInferenceAdapterType = IAInferenceAdapterType.Mistral;

            _adapter = adapterFactory.CreateAdapter<HotelVector>();
            _aIInferenceService = aIInferenceService;
        }

        public async Task<float[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
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

        public async Task UpsertDataAsync(HotelVector entity)
        {
            var embedding = await _aIInferenceService.GenerateEmbeddingAsync(entity.Description, _eIAInferenceAdapterType);

            entity.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);

            await _adapter.UpsertDataAsync(nameCollection, entity);
        }

        public async Task UpsertDatasAsync(HotelVector[] listEntity)
        {
            var hotelVectors = new List<HotelVector>();

            foreach (HotelVector hotel in listEntity)
            {
                if (!await _adapter.Exists(nameCollection, hotel.DataKey))
                {
                    var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, _eIAInferenceAdapterType);

                    hotel.Embedding = EmbeddingHelper.ConvertToReadOnlyMemory(embedding);

                    hotelVectors.Add(hotel);
                }
            }
            if (hotelVectors.Count > 0)
            {
                await _adapter.UpsertDatasAsync(nameCollection, hotelVectors.ToArray());
            }
        }

        public async Task<ServiceResponse<HotelVector[]>> VectorizedSearchAsync(SearchCriteria searchCriteria)
        {
            ServiceResponse<HotelVector[]> response = new ServiceResponse<HotelVector[]>();
            try
            {
                //Get semantic search 
                var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchCriteria.SearchTextCriteria, _eIAInferenceAdapterType);

                var hotelsVector = await _adapter.VectorizedSearchAsync(nameCollection, embeddingSearchText, searchCriteria);
                response.Success = true;
                response.Data = hotelsVector;
            }
            catch (Exception ex)
            {
#pragma warning disable S6776
                response.Success = false;
                response.Message = ex.Message;
                // NOSONAR
                response.Errors = new List<ErrorResponse>() { new ErrorResponse() { Message = ex.Message }, new ErrorResponse() { Message = ex.StackTrace ?? string.Empty } };
#pragma warning restore S6776
            }
            return response;
        }

        public async Task<ServiceResponse<HotelVector[]>> SearchAndAnalyzePluginAsync(string searchText)
        {
            ServiceResponse<HotelVector[]> response = new ServiceResponse<HotelVector[]>();
            try
            {
                //Get semantic search 
                var embeddingSearchText = await _aIInferenceService.GenerateEmbeddingAsync(searchText, _eIAInferenceAdapterType);

                var resultIA = await _adapter.SearchAndAnalyzePluginAsync(nameCollection, searchText, embeddingSearchText);

                response.Success = true;
                response.Data = resultIA;
            }
            catch (Exception ex)
            {
#pragma warning disable S6776
                 
                response.Success = false;
                response.Message = ex.Message;
                
                // NOSONAR
                response.Errors = new List<ErrorResponse>() { new ErrorResponse() { Message = ex.Message }, new ErrorResponse() { Message = ex.StackTrace ?? string.Empty } };
#pragma warning restore S6776
            }
            return response;
        }

        public async Task DeleteAsync(long dataKey)
        {
            await _adapter.DeleteAsync(nameCollection, dataKey);
        }


    }
}