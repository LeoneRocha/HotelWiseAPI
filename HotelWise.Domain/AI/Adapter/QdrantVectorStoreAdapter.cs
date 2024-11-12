using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.AI.Adapter
{
    public class QdrantVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector> where TVector : IDataVector
    {
        private readonly IApplicationConfig _applicationConfig;
        private readonly IVectorStore _vectorStore;
        private IVectorStoreRecordCollection<ulong, TVector>? collection;

        public QdrantVectorStoreAdapter(IApplicationConfig applicationConfig, IVectorStore vectorStore)
        {
            _applicationConfig = applicationConfig;
            _vectorStore = vectorStore;
        }
        private async Task LoadCollection(string nameCollection)
        {
            collection = _vectorStore.GetCollection<ulong, TVector>(nameCollection);

            await CreateCollection();
        }
        private async Task CreateCollection()
        {
            await collection!.CreateCollectionIfNotExistsAsync();
        } 

        public async Task UpsertDataAsync(string nameCollection, TVector dataVector)
        {
            await LoadCollection(nameCollection);
            // Create the collection if it doesn't exist yet. 

            await collection!.UpsertAsync(dataVector);
        } 

        public async Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors)
        {
            await LoadCollection(nameCollection); 

            // Create a record and generate a vector for the description using your chosen embedding generation implementation.
            // Just showing a placeholder embedding generation method here for brevity.
            foreach (TVector dataVector in dataVectors)
            {
                await collection!.UpsertAsync(dataVector);
                //collection.UpsertBatchAsync 
            }
        }

        public async Task<TVector?> GetByKey(string nameCollection, ulong dataKey)
        {
            await LoadCollection(nameCollection);
            // Retrieve the upserted record.
            TVector? retrievedHotel = await collection!.GetAsync(dataKey);

            return retrievedHotel;
        }

        public async Task<bool> Exists(string nameCollection, ulong dataKey)
        {
            await LoadCollection(nameCollection);
            // Retrieve the upserted record.
            TVector? retrievedHotel = await collection!.GetAsync(dataKey);

            return !EqualityComparer<TVector>.Default.Equals(retrievedHotel, default(TVector));
        }

        public async Task<TVector[]> SearchDatasAsync(string nameCollection, float[] searchEmbedding)
        {
            await LoadCollection(nameCollection);
            // Generate a vector for your search text, using your chosen embedding generation implementation.
            // Just showing a placeholder method here for brevity.  

            var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);

            // Do the search.
            var searchResult = await collection!.VectorizedSearchAsync(searchEmbeddingCriteria, new()
            {
                Top = 2,
                //Filter = new VectorSearchFilter().AnyTagEqualTo(nameof(TVector.Tags), "classe:alta") // ENRIQUECANDO O DADO PARA TORNAR MAIS RELEVANT
            });

            var dataSearchResult = searchResult.Results.ToBlockingEnumerable().ToArray();

            List<TVector> dataVectors = new List<TVector>();

            foreach (var item in dataSearchResult)
            {
                var addVect = item.Record;
                addVect.Score = item.Score.GetValueOrDefault();
                dataVectors.Add(addVect);
            }
            //// Inspect the returned hotels.
            //HotelVector hotel = searchResult.First().Record;               
            return dataVectors.ToArray();
        }

    }
}