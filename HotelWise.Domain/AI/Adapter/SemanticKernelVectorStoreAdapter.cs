using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Mistral.SDK.DTOs;
using Mistral.SDK;
using Qdrant.Client;

namespace HotelWise.Domain.AI.Adapter
{
    public class SemanticKernelVectorStoreAdapter : IVectorStoreAdapter
    {
        private readonly IVectorStoreRecordCollection<ulong, HotelVector> collection;
        private readonly IVectorStoreSettingsDto _vectorStoreSettingsDto;


        public SemanticKernelVectorStoreAdapter(IVectorStoreSettingsDto vectorStoreSettingsDto)
        {
            _vectorStoreSettingsDto = vectorStoreSettingsDto;

#pragma warning disable SKEXP0020
            // Create a Qdrant VectorStore object
            var vectorStore = new QdrantVectorStore(new QdrantClient(_vectorStoreSettingsDto.Host, _vectorStoreSettingsDto.Port));

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
                //collection.UpsertBatchAsync 
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
            var client = new MistralClient(_vectorStoreSettingsDto.ApiKeyEmbeddings);
            var request = new EmbeddingRequest(
                ModelDefinitions.MistralEmbed,
                new List<string>() { "Hello world" },
                EmbeddingRequest.EncodingFormatEnum.Float);
            var response = await client.Embeddings.GetEmbeddingsAsync(request);

            //---PRECISO CONTINUAR
            //TODO: 
            //-- CRIAR UM ADAPTER E SERVICE SEMELHANTE AO CROC o emband nao vai ficar na classe do quadrand ele ja vei receber tudo ja embeend
            //-- REFATORA PARA O _vectorStoreService chamar o NOVO SERVICE DO MISTRAL QUE VAI SUBSTITUIR O GROQ pára gerar o embbaeding e chamar o COmpletion 


            throw new NotImplementedException();
        }
    }
}