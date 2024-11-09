using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;

namespace HotelWise.Service.AI
{
    public class VectorStoreService : IVectorStoreService
    {
        private readonly IVectorStoreAdapter _adapter;

        public VectorStoreService(IVectorStoreAdapterFactory adapterFactory)
        {
            _adapter = adapterFactory.CreateAdapter();
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
                hotelVectors.Add(new HotelVector()
                {
                    HotelId = (ulong)hotel.HotelId,
                    HotelName = hotel.HotelName,
                    Description = hotel.Description,
                    Tags = hotel.Tags,
                    DescriptionEmbedding = await _adapter.GenerateEmbeddingAsync(hotel.Description)
                });
            }
            await _adapter.UpsertHotelAsync(hotelVectors.ToArray());
        }
    }
}