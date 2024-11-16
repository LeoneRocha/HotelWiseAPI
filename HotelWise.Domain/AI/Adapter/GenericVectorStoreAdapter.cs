using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using System.Diagnostics;

namespace HotelWise.Domain.AI.Adapter
{
    public class GenericVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector> where TVector : IDataVector
    {
        private readonly IApplicationIAConfig _applicationConfig;
        private readonly IVectorStore _vectorStore;
        private IVectorStoreRecordCollection<ulong, TVector>? collection;
        private readonly Kernel _kernel;
        private readonly Serilog.ILogger _logger;

        public GenericVectorStoreAdapter(Serilog.ILogger logger,
            IApplicationIAConfig applicationConfig
            , IVectorStore vectorStore
            , Kernel kernel)
        {
            _applicationConfig = applicationConfig;
            _vectorStore = vectorStore;
            _kernel = kernel;
            _logger = logger;
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

        public async Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding)
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

        public async Task<TVector[]> SearchPluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding)
        {
            List<TVector> dataVectors = new List<TVector>();
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                _logger.Information("SearchPluginAsync: {time}", DateTime.UtcNow);
                await LoadCollection(nameCollection);

#pragma warning disable SKEXP0001

                ITextEmbeddingGenerationService embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
                var vectorStoreTextSearch = new VectorStoreTextSearch<TVector>(collection!, embeddingService);

#pragma warning restore SKEXP0001

                string pluginName = CreatePlugin(vectorStoreTextSearch);

                string template = CreateTemplate(pluginName);

                var results = await GetVectorsResults(searchEmbedding);

                _logger.Information("VectorizedSearchAsync : {dataSearchResult}", results.DataSearchResult);

                KernelArguments arguments = CreateArgments(searchQuery, results.SearchResult);

                HandlebarsPromptTemplateFactory promptTemplateFactory = new();

                string templateResult = await RenderPrompt(searchQuery, template, results, promptTemplateFactory);

                IAsyncEnumerable<StreamingKernelContent> result2 = await InvokePrompt(template, arguments, promptTemplateFactory);

                await foreach (var message in result2)
                {
                    _logger.Information("Result t: {message}", message);
                }
                stopwatch.Stop();
                _logger.Information("SearchPluginAsync completed in: {elapsed} (hh:mm:ss)", TimeFormatter.FormatElapsedTime(stopwatch.Elapsed));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred in SearchPluginAsync at: {Message} at: {time}", ex.Message, DateTime.UtcNow);
            }
            return dataVectors.ToArray();
        }

        private static string CreateTemplate(string pluginName)
        {
            string template = """
                    {{query}}
                    {{#with (SearchPlugin-GetTextSearchResults query)}}  
                        {{#each this}}  
                            {{#if Value}}
                                Hotel Name: {{HotelName}}
                                Description: {{Description}}
                                -----------------
                            {{else}}
                                Text: Not result               
                                -----------------
                            {{/if}}
                        {{/each}}  
                    {{/with}}  
                """;
            template = template.Replace("SearchPlugin", pluginName);
            return template;
        }

#pragma warning disable SKEXP0001
        private string CreatePlugin(VectorStoreTextSearch<TVector> vectorStoreTextSearch)
        {
            // Obter o nome da classe de TVector
            string vectorClassName = typeof(TVector).Name;
            string pluginName = $"SearchPlugin{vectorClassName}";
            var searchPlugin = vectorStoreTextSearch.CreateWithGetTextSearchResults(pluginName);
            // Verificar se o plugin já existe antes de adicionar
            if (!_kernel.Plugins.Any(p => p.Name == searchPlugin.Name))
            {
                _kernel.Plugins.Add(searchPlugin);
            }

            return pluginName;
        }
#pragma warning restore SKEXP0001

        private async Task<IAsyncEnumerable<StreamingKernelContent>> InvokePrompt(string template, KernelArguments arguments, HandlebarsPromptTemplateFactory promptTemplateFactory)
        {
            try
            {
                var resultKernel = await _kernel.InvokePromptAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);
                var result2 = _kernel.InvokePromptStreamingAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);

                return result2;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred in InvokePrompt at: {Message} at: {time}", ex.Message, DateTime.UtcNow);
                throw;
            }
        }

        private static KernelArguments CreateArgments(string searchQuery, VectorSearchResults<TVector> searchResult)
        {
            return new KernelArguments
            {
                { "query", searchQuery }
                ,{ "results", searchResult}
            };
        }

        private async Task<string> RenderPrompt(string searchQuery, string template, (VectorSearchResults<TVector> SearchResult, VectorSearchResult<TVector>[] DataSearchResult) results, HandlebarsPromptTemplateFactory promptTemplateFactory)
        {

            try
            {
                string templateResult = await promptTemplateFactory.Create(new PromptTemplateConfig()
                {
                    Template = template,
                    TemplateFormat = HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat,
                    InputVariables = new List<InputVariable> {
                    new InputVariable() { Name = "query", Default = searchQuery },
                    new InputVariable() { Name = "results", Default = results.SearchResult }
                }
                }).RenderAsync(_kernel);

                _logger.Information("Rendered Prompt: {templateResult}", templateResult);

                return templateResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred in RenderPrompt at: {Message} at: {time}", ex.Message, DateTime.UtcNow);
                throw;
            }
        }

        private async Task<(VectorSearchResults<TVector> SearchResult, VectorSearchResult<TVector>[] DataSearchResult)> GetVectorsResults(float[] searchEmbedding)
        {
            var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);
            // Do the search.
            var searchResult = await collection!.VectorizedSearchAsync(searchEmbeddingCriteria, new()
            {
                Top = 2,
                //Filter = new VectorSearchFilter().AnyTagEqualTo(nameof(TVector.Tags), "classe:alta") // ENRIQUECANDO O DADO PARA TORNAR MAIS RELEVANT
            });
            var dataSearchResult = searchResult.Results.ToBlockingEnumerable().ToArray();

            return (SearchResult: searchResult!, DataSearchResult: dataSearchResult);
        }
    }
}