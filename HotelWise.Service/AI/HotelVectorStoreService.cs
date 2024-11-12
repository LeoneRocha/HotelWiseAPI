using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;

namespace HotelWise.Service.AI
{
    public class HotelVectorStoreService : IVectorStoreService<Hotel>
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
            return await _aIInferenceService.GenerateEmbeddingAsync(text, Domain.Enuns.EIAInferenceAdapterType.Mistral);
        }

        public async Task<Hotel?> GetById(long dataKey)
        {
            var hotelVector = await _adapter.GetByKey(nameCollection, (ulong)dataKey);

            if (hotelVector != null)
            {
                return new Hotel()
                {
                    Description = hotelVector.Description,
                    HotelId = dataKey,
                    HotelName = hotelVector.HotelName
                };
            }
            return null;
        }

        public async Task UpsertDataAsync(Hotel hotel)
        {
            var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, Domain.Enuns.EIAInferenceAdapterType.Mistral);

            var hotelVector = new HotelVector()
            {
                HotelId = (ulong)hotel.HotelId,
                HotelName = hotel.HotelName,
                Description = hotel.Description,
                Tags = hotel.Tags.ToList(),
                DescriptionEmbedding = ConvertToReadOnlyMemory(embedding)
            };

            await _adapter.UpsertDataAsync(nameCollection, hotelVector);
        }

        public async Task UpsertDatasAsync(Hotel[] hotels)
        {
            var hotelVectors = new List<HotelVector>();

            foreach (Hotel hotel in hotels)
            {
                if (!await _adapter.Exists(nameCollection, (ulong)hotel.HotelId))
                {
                    var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, Domain.Enuns.EIAInferenceAdapterType.Mistral);

                    hotelVectors.Add(new HotelVector()
                    {
                        HotelId = (ulong)hotel.HotelId,
                        HotelName = hotel.HotelName,
                        Description = hotel.Description,
                        Tags = hotel.Tags.ToList(),
                        DescriptionEmbedding = ConvertToReadOnlyMemory(embedding)
                    });
                }
            }
            await _adapter.UpsertDatasAsync(nameCollection, hotelVectors.ToArray());
        }
        public async Task<Hotel[]> SearchDatasAsync(string searchText)
        {
            //Get semantic search 
            var hotelsVector = await _adapter.SearchDatasAsync(nameCollection, searchText);

            //Enriquecer com Interferencia IA  TODO:

            // Mapear para novo objeto e retonar novo objeto TODO:

            var resultHotels = new List<Hotel>();
            foreach (var hotelVector in hotelsVector)
            {
                resultHotels.Add(new Hotel()
                {
                    Description = hotelVector.Description,
                    HotelId = (long)hotelVector.HotelId,
                    HotelName = hotelVector.HotelName
                });
            }
            return resultHotels.ToArray();
        }

        private ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings)
        {
            var resultMen = new ReadOnlyMemory<float>(embeddings);
            return resultMen;
        }

    }
}