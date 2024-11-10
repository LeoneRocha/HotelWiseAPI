using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;
using System.Runtime.InteropServices;

namespace HotelWise.Service.AI
{
    public class VectorStoreService : IVectorStoreService
    {
        private readonly IVectorStoreAdapter _adapter;
        private readonly IAIInferenceService _aIInferenceService;


        public VectorStoreService(IVectorStoreAdapterFactory adapterFactory, IAIInferenceService aIInferenceService)
        {
            _adapter = adapterFactory.CreateAdapter();
            _aIInferenceService = aIInferenceService;   
        }

        public async Task<decimal[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, Domain.Enuns.EIAInferenceAdapterType.Mistral);
        }

        public async Task<Hotel?> GetById(long hotelId)
        {
            var hotelVector = await _adapter.GetById((ulong)hotelId);

            if (hotelVector != null)
            {
                return new Hotel()
                {
                    Description = hotelVector.Description,
                    HotelId = hotelId,
                    HotelName = hotelVector.HotelName
                };
            }
            return null;

        }

        public async Task<Hotel[]> SearchHotelsAsync(string searchText)
        {
            //Get semantic search 
            var hotelsVector = await _adapter.SearchHotelsAsync(searchText);

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

        public async Task UpsertHotelAsync(Hotel[] hotels)
        {
            var hotelVectors = new List<HotelVector>();

            foreach (Hotel hotel in hotels)
            {
                var embedding = await _aIInferenceService.GenerateEmbeddingAsync(hotel.Description, Domain.Enuns.EIAInferenceAdapterType.Mistral);
                 
                hotelVectors.Add(new HotelVector()
                {
                    HotelId = (ulong)hotel.HotelId,
                    HotelName = hotel.HotelName,
                    Description = hotel.Description,
                    Tags = hotel.Tags,
                    DescriptionEmbedding = ConvertToReadOnlyMemory(embedding)
                });
            }
            await _adapter.UpsertHotelAsync(hotelVectors.ToArray());
        }

        private ReadOnlyMemory<float> ConvertToReadOnlyMemory(decimal[] decimalArray)
        {
            if (decimalArray == null)
                throw new ArgumentNullException(nameof(decimalArray));

            //float[] floatArray = decimalArray.Select(d => (float)d).ToArray(); 
            Span<float> floatSpan = MemoryMarshal.Cast<decimal, float>(decimalArray.AsSpan());//Porque e melhor usar MemoryMarshal.Cast = performace
            var resultMen = new ReadOnlyMemory<float>(floatSpan.ToArray());
             
            return resultMen;
        }
        
    }
}