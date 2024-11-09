using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;

namespace HotelWise.Domain.AI.Adapter
{
    public class SemanticKernelVectorStoreAdapter : IVectorStoreAdapter
    {
        private readonly IVectorStoreRecordCollection<ulong, HotelVector> collection;

        public SemanticKernelVectorStoreAdapter()
        {

#pragma warning disable SKEXP0020
            // Create a Qdrant VectorStore object
            var vectorStore = new QdrantVectorStore(new QdrantClient("localhost"));

            // Choose a collection from the database and specify the type of key and record stored in it via Generic parameters.
            collection = vectorStore.GetCollection<ulong, HotelVector>("skhotels");

#pragma warning restore SKEXP0020 
        }

        public async Task UpsertHotelAsync(HotelVector[] hotels)
        {
            // Create the collection if it doesn't exist yet.
            await collection.CreateCollectionIfNotExistsAsync();

            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            // Just showing a placeholder embedding generation method here for brevity.
            foreach (HotelVector hotelV in hotels)
            {
                await collection.UpsertAsync(hotelV);
            }
        }
        public async Task<HotelVector?> GetById(ulong hotelId)
        {
            // Retrieve the upserted record.
            HotelVector? retrievedHotel = await collection.GetAsync(hotelId);

            return retrievedHotel;
        }

        public async Task<HotelVector[]> SearchHotelsAsync(string searchText)
        {
            // Generate a vector for your search text, using your chosen embedding generation implementation.
            // Just showing a placeholder method here for brevity.
            var searchEmbedding = await GenerateEmbeddingAsync(searchText);

            // Do the search.
            //var searchResult = await collection.VectorizedSearchAsync(searchEmbedding, new() { Top = 1 }).Results.ToListAsync();

            //// Inspect the returned hotels.
            //HotelVector hotel = searchResult.First().Record;               

            return [];
        }

        public async Task<ReadOnlyMemory<float>?> GenerateEmbeddingAsync(string text)
        {
            //TODO: 
            throw new NotImplementedException();
        }
    }
}