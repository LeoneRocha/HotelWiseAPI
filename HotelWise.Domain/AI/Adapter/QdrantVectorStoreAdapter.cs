using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.AI.Adapter
{
    public class QdrantVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector>
    {
        private readonly IApplicationConfig _applicationConfig;
        private readonly IVectorStore _vectorStore;
        private IVectorStoreRecordCollection<ulong, TVector>? collection;

        public QdrantVectorStoreAdapter(IApplicationConfig applicationConfig, IVectorStore vectorStore)
        {
            _applicationConfig = applicationConfig;
            _vectorStore = vectorStore;
        }

        private void loadCollection(string nameCollection)
        {
            collection = _vectorStore.GetCollection<ulong, TVector>(nameCollection);
        }

        public async Task UpsertDataAsync(string nameCollection, TVector dataVector)
        {
            loadCollection(nameCollection);
            // Create the collection if it doesn't exist yet.
            await collection!.CreateCollectionIfNotExistsAsync();

            await collection.UpsertAsync(dataVector);
        }
        public async Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors)
        {
            loadCollection(nameCollection);
            // Create the collection if it doesn't exist yet.
            await collection!.CreateCollectionIfNotExistsAsync();

            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            // Just showing a placeholder embedding generation method here for brevity.
            foreach (TVector dataVector in dataVectors)
            {
                await collection.UpsertAsync(dataVector);
                //collection.UpsertBatchAsync 
            }
        }
        public async Task<TVector?> GetByKey(string nameCollection, ulong dataKey)
        {
            loadCollection(nameCollection);
            // Retrieve the upserted record.
            TVector? retrievedHotel = await collection!.GetAsync(dataKey);

            return retrievedHotel;
        }

        public async Task<bool> Exists(string nameCollection, ulong dataKey)
        {
            loadCollection(nameCollection);
            // Retrieve the upserted record.
            TVector? retrievedHotel = await collection!.GetAsync(dataKey);

            return retrievedHotel != null;
        }

        public async Task<TVector[]> SearchDatasAsync(string nameCollection, string searchText)
        {
            loadCollection(nameCollection);
            // Generate a vector for your search text, using your chosen embedding generation implementation.
            // Just showing a placeholder method here for brevity.
            //var searchEmbedding = await GenerateEmbeddingAsync(searchText);
            TVector? retrievedData = await collection!.GetAsync(1);
            // Do the search.
            //var searchResult = await collection.VectorizedSearchAsync(searchEmbedding, new() { Top = 1 }).Results.ToListAsync();
            //// Inspect the returned hotels.
            //HotelVector hotel = searchResult.First().Record;               
            return [];
        }
    }
}