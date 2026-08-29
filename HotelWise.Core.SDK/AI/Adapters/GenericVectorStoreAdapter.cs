#if NET8_0_OR_GREATER
using System.Diagnostics;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Helpers;
using HotelWise.Core.SDK.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter genérico de vector store.
/// </summary>
public class GenericVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector> where TVector : class, IDataVector
{
    private readonly VectorStore _vectorStore;
    private VectorStoreCollection<ulong, TVector>? _collection;
    private readonly Kernel _kernel;
    private readonly Serilog.ILogger _logger;

    public GenericVectorStoreAdapter(
        Serilog.ILogger logger,
        IApplicationIAConfig applicationConfig,
        VectorStore vectorStore,
        Kernel kernel)
    {
        _vectorStore = vectorStore;
        _kernel = kernel;
        _logger = logger;
    }

    private async Task LoadCollection(string nameCollection)
    {
        _collection = _vectorStore.GetCollection<ulong, TVector>(nameCollection);
        await CreateCollection();
    }

    private async Task CreateCollection() => await _collection!.EnsureCollectionExistsAsync();

    public async Task UpsertDataAsync(string nameCollection, TVector dataVector)
    {
        await LoadCollection(nameCollection);
        await _collection!.UpsertAsync(dataVector);
    }

    public async Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors)
    {
        await LoadCollection(nameCollection);
        foreach (TVector dataVector in dataVectors)
        {
            await _collection!.UpsertAsync(dataVector);
        }
    }

    public async Task<TVector?> GetByKey(string nameCollection, ulong dataKey)
    {
        await LoadCollection(nameCollection);
        return await _collection!.GetAsync(dataKey);
    }

    public async Task<bool> Exists(string nameCollection, ulong dataKey)
    {
        await LoadCollection(nameCollection);
        TVector? retrieved = await _collection!.GetAsync(dataKey);
        return !EqualityComparer<TVector>.Default.Equals(retrieved, default);
    }

    public async Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria)
    {
        await LoadCollection(nameCollection);
        var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);
        VectorSearchOptions<TVector> vectorSearchOptions = CreateOptions(searchCriteria);
        var searchResult = _collection!.SearchAsync(searchEmbeddingCriteria, searchCriteria.MaxHotelRetrieve, vectorSearchOptions);

        List<TVector> dataVectors = new List<TVector>();
        await foreach (var item in searchResult)
        {
            var addVect = item.Record;
            addVect.Score = item.Score.GetValueOrDefault();
            dataVectors.Add(addVect);
        }
        return dataVectors.ToArray();
    }

    private static VectorSearchOptions<TVector> CreateOptions(SearchCriteria searchCriteria)
    {
        var vectorSearchOptions = new VectorSearchOptions<TVector>()
        {
            VectorProperty = r => r.Embedding
        };

        if (searchCriteria.TagsCriteria.Length > 0)
        {
            var tagsCriteria = searchCriteria.TagsCriteria.ToList();
            vectorSearchOptions = new VectorSearchOptions<TVector>()
            {
                Filter = r => r.Tags.Any(tag => tagsCriteria.Contains(tag)),
                VectorProperty = r => r.Embedding
            };
        }
        return vectorSearchOptions;
    }

    public async Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding)
    {
        List<TVector> dataVectors = new List<TVector>();
        var stopwatch = Stopwatch.StartNew();
        InsertLogStarterSearchPluginAsync();
        await LoadCollection(nameCollection);

        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
            _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var vectorStoreTextSearch = new VectorStoreTextSearch<TVector>(_collection!, embeddingGenerator);
        string pluginName = CreatePlugin(vectorStoreTextSearch);
        string template = CreateTemplate(pluginName);
        var results = await GetVectorsResults(searchEmbedding);
        InsertLogVectorizedSearchAsync(results);
        KernelArguments arguments = CreateArguments(searchQuery, results);
        HandlebarsPromptTemplateFactory promptTemplateFactory = new();
        string templateResult = await RenderPrompt(searchQuery, template, results, promptTemplateFactory);
        _logger.Information("SearchAndAnalyzePluginAsync - Rendered Prompt: {templateResult}", templateResult);
        IAsyncEnumerable<StreamingKernelContent> result2 = await InvokePrompt(template, arguments, promptTemplateFactory);
        await foreach (var message in result2)
        {
            _logger.Information("Result IA : {message}", message);
        }
        stopwatch.Stop();
        _logger.Information("SearchPluginAsync completed in: {elapsed} (hh:mm:ss)", TimeFormatter.FormatElapsedTime(stopwatch.Elapsed));
        return dataVectors.ToArray();
    }

    private void InsertLogVectorizedSearchAsync(VectorSearchResult<TVector>[] results) =>
        _logger.Information("VectorizedSearchAsync : {dataSearchResult}", results);

    private void InsertLogStarterSearchPluginAsync() =>
        _logger.Information("SearchPluginAsync: {time}", DateTime.UtcNow);

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
        return template.Replace("SearchPlugin", pluginName);
    }

#pragma warning disable SKEXP0001
    private string CreatePlugin(VectorStoreTextSearch<TVector> vectorStoreTextSearch)
    {
        string vectorClassName = typeof(TVector).Name;
        string pluginName = $"SearchPlugin{vectorClassName}";
        var searchPlugin = vectorStoreTextSearch.CreateWithGetTextSearchResults(pluginName);
        if (!_kernel.Plugins.Any(p => p.Name == searchPlugin.Name))
        {
            _kernel.Plugins.Add(searchPlugin);
        }
        return pluginName;
    }
#pragma warning restore SKEXP0001

    private async Task<IAsyncEnumerable<StreamingKernelContent>> InvokePrompt(string template, KernelArguments arguments, HandlebarsPromptTemplateFactory promptTemplateFactory)
    {
        var resultKernel = await _kernel.InvokePromptAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);
        _logger.Information("InvokePrompt  - InvokePromptAsync: {templateResult}", resultKernel);
        return _kernel.InvokePromptStreamingAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);
    }

    private static KernelArguments CreateArguments(string searchQuery, VectorSearchResult<TVector>[] searchResult) =>
        new KernelArguments
        {
            { "query", searchQuery },
            { "results", searchResult }
        };

    private async Task<string> RenderPrompt(string searchQuery, string template, VectorSearchResult<TVector>[] results, HandlebarsPromptTemplateFactory promptTemplateFactory)
    {
        string templateResult = await promptTemplateFactory.Create(new PromptTemplateConfig()
        {
            Template = template,
            TemplateFormat = HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat,
            InputVariables = new List<InputVariable> {
                new InputVariable() { Name = "query", Default = searchQuery },
                new InputVariable() { Name = "results", Default = results }
            }
        }).RenderAsync(_kernel);
        _logger.Information("Rendered Prompt: {templateResult}", templateResult);
        return templateResult;
    }

    private async Task<VectorSearchResult<TVector>[]> GetVectorsResults(float[] searchEmbedding)
    {
        var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);
        var searchResult = _collection!.SearchAsync(searchEmbeddingCriteria, top: 2, new VectorSearchOptions<TVector>
        {
            VectorProperty = r => r.Embedding
        });
        var dataSearchResult = new List<VectorSearchResult<TVector>>();
        await foreach (var item in searchResult)
        {
            dataSearchResult.Add(item);
        }
        return dataSearchResult.ToArray();
    }

    public async Task DeleteAsync(string nameCollection, long dataKey)
    {
        await LoadCollection(nameCollection);
        await _collection!.DeleteAsync((ulong)dataKey);
    }
}
#endif
