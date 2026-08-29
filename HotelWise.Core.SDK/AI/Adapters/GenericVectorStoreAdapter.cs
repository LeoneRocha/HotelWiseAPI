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
/// Adapter genérico de vector store baseado em Microsoft.Extensions.VectorData e Semantic Kernel.
/// Implementa <see cref="IVectorStoreAdapter{TVector}"/> para upsert, leitura, busca semântica
/// e análise assistida por plugin LLM no pipeline RAG.
/// </summary>
/// <typeparam name="TVector">Tipo do registro vetorial, implementando <see cref="IDataVector"/>.</typeparam>
/// <example>
/// <code>
/// var adapter = new GenericVectorStoreAdapter&lt;HotelVector&gt;(logger, appConfig, vectorStore, kernel);
/// await adapter.UpsertDataAsync("hotels", hotelVector);
/// var results = await adapter.VectorizedSearchAsync("hotels", embedding, criteria);
/// </code>
/// </example>
public class GenericVectorStoreAdapter<TVector> : IVectorStoreAdapter<TVector> where TVector : class, IDataVector
{
    /// <summary>
    /// Instância do vector store injetada.
    /// </summary>
    private readonly VectorStore _vectorStore;

    /// <summary>
    /// Kernel do Semantic Kernel para plugins e prompts.
    /// </summary>
    private readonly Kernel _kernel;

    /// <summary>
    /// Logger estruturado.
    /// </summary>
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Inicializa o adapter com logger, configuração de IA, vector store e kernel.
    /// </summary>
    /// <param name="logger">Logger Serilog.</param>
    /// <param name="applicationConfig">Configuração agregada de IA (reservada para extensões).</param>
    /// <param name="vectorStore">Vector store do Semantic Kernel / Extensions.AI.</param>
    /// <param name="kernel">Kernel do Semantic Kernel.</param>
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

    /// <summary>
    /// Carrega (e cria se necessário) a coleção tipada pelo nome.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <returns>Coleção tipada pronta para uso.</returns>
    private async Task<VectorStoreCollection<ulong, TVector>> LoadCollection(string nameCollection)
    {
        var collection = _vectorStore.GetCollection<ulong, TVector>(nameCollection);
        await collection.EnsureCollectionExistsAsync();
        return collection;
    }

    /// <summary>
    /// Insere ou atualiza um único registro vetorial na coleção.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="dataVector">Registro a persistir.</param>
    /// <returns>Tarefa que conclui quando o upsert for finalizado.</returns>
    /// <example>
    /// <code>
    /// await adapter.UpsertDataAsync("hotels", vector);
    /// </code>
    /// </example>
    public async Task UpsertDataAsync(string nameCollection, TVector dataVector)
    {
        var collection = await LoadCollection(nameCollection);
        await collection.UpsertAsync(dataVector);
    }

    /// <summary>
    /// Insere ou atualiza múltiplos registros vetoriais na coleção.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="dataVectors">Registros a persistir.</param>
    /// <returns>Tarefa que conclui quando todos os upserts forem finalizados.</returns>
    public async Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors)
    {
        var collection = await LoadCollection(nameCollection);
        foreach (TVector dataVector in dataVectors)
        {
            await collection.UpsertAsync(dataVector);
        }
    }

    /// <summary>
    /// Obtém um registro pela chave na coleção.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="dataKey">Chave do registro.</param>
    /// <returns>Registro encontrado, ou <c>null</c>.</returns>
    public async Task<TVector?> GetByKey(string nameCollection, ulong dataKey)
    {
        var collection = await LoadCollection(nameCollection);
        return await collection.GetAsync(dataKey);
    }

    /// <summary>
    /// Verifica se um registro com a chave existe na coleção.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="dataKey">Chave do registro.</param>
    /// <returns><c>true</c> se existir; caso contrário, <c>false</c>.</returns>
    public async Task<bool> Exists(string nameCollection, ulong dataKey)
    {
        var collection = await LoadCollection(nameCollection);
        TVector? retrieved = await collection.GetAsync(dataKey);
        return !EqualityComparer<TVector>.Default.Equals(retrieved, default);
    }

    /// <summary>
    /// Executa busca vetorial por similaridade com embedding e critérios opcionais de tags.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="searchEmbedding">Vetor de embedding da consulta.</param>
    /// <param name="searchCriteria">Critérios (limite e tags).</param>
    /// <returns>Registros similares com <see cref="IDataVector.Score"/> preenchido.</returns>
    /// <example>
    /// <code>
    /// var hits = await adapter.VectorizedSearchAsync("hotels", emb, new SearchCriteria { MaxHotelRetrieve = 5 });
    /// </code>
    /// </example>
    public async Task<TVector[]> VectorizedSearchAsync(string nameCollection, float[] searchEmbedding, SearchCriteria searchCriteria)
    {
        var collection = await LoadCollection(nameCollection);
        var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);
        VectorSearchOptions<TVector> vectorSearchOptions = CreateOptions(searchCriteria);
        var searchResult = collection.SearchAsync(searchEmbeddingCriteria, searchCriteria.MaxHotelRetrieve, vectorSearchOptions);

        List<TVector> dataVectors = new List<TVector>();
        await foreach (var item in searchResult)
        {
            var addVect = item.Record;
            addVect.Score = item.Score.GetValueOrDefault();
            dataVectors.Add(addVect);
        }
        return dataVectors.ToArray();
    }

    /// <summary>
    /// Monta opções de busca vetorial, incluindo filtro por tags quando informado.
    /// </summary>
    /// <param name="searchCriteria">Critérios de busca.</param>
    /// <returns>Opções de <see cref="VectorSearchOptions{TRecord}"/>.</returns>
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

    /// <summary>
    /// Busca vetorial combinada com análise via plugin Handlebars do Semantic Kernel.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="searchQuery">Consulta textual.</param>
    /// <param name="searchEmbedding">Embedding da consulta.</param>
    /// <returns>Registros resultantes (lista populada conforme fluxo do plugin).</returns>
    /// <example>
    /// <code>
    /// var analyzed = await adapter.SearchAndAnalyzePluginAsync("hotels", "praia", emb);
    /// </code>
    /// </example>
    public async Task<TVector[]> SearchAndAnalyzePluginAsync(string nameCollection, string searchQuery, float[] searchEmbedding)
    {
        List<TVector> dataVectors = new List<TVector>();
        var stopwatch = Stopwatch.StartNew();
        InsertLogStarterSearchPluginAsync();
        var collection = await LoadCollection(nameCollection);

        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
            _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        var vectorStoreTextSearch = new VectorStoreTextSearch<TVector>(collection, embeddingGenerator);
        string pluginName = CreatePlugin(vectorStoreTextSearch);
        string template = CreateTemplate(pluginName);
        var results = await GetVectorsResults(collection, searchEmbedding);
        InsertLogVectorizedSearchAsync(results);
        KernelArguments arguments = CreateArguments(searchQuery, results);
        HandlebarsPromptTemplateFactory promptTemplateFactory = new();
        string templateResult = await RenderPrompt(searchQuery, template, results, promptTemplateFactory);
        _logger.Information("SearchAndAnalyzePluginAsync - Rendered Prompt: {TemplateResult}", templateResult);
        IAsyncEnumerable<StreamingKernelContent> result2 = await InvokePrompt(template, arguments, promptTemplateFactory);
        await foreach (var message in result2)
        {
            _logger.Information("Result IA : {Message}", message);
        }
        stopwatch.Stop();
        _logger.Information("SearchPluginAsync completed in: {Elapsed} (hh:mm:ss)", TimeFormatter.FormatElapsedTime(stopwatch.Elapsed));
        return dataVectors.ToArray();
    }

    /// <summary>
    /// Registra no log o resultado da busca vetorial intermediária.
    /// </summary>
    /// <param name="results">Resultados da busca.</param>
    private void InsertLogVectorizedSearchAsync(VectorSearchResult<TVector>[] results) =>
        _logger.Information("VectorizedSearchAsync : {DataSearchResult}", results);

    /// <summary>
    /// Registra no log o início da busca com plugin.
    /// </summary>
    private void InsertLogStarterSearchPluginAsync() =>
        _logger.Information("SearchPluginAsync: {Time}", DateTime.UtcNow);

    /// <summary>
    /// Cria o template Handlebars do plugin de busca textual.
    /// </summary>
    /// <param name="pluginName">Nome do plugin registrado no kernel.</param>
    /// <returns>Template Handlebars como string.</returns>
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
    /// <summary>
    /// Cria e registra o plugin de text search no kernel, se ainda não existir.
    /// </summary>
    /// <param name="vectorStoreTextSearch">Fonte de busca textual vetorial.</param>
    /// <returns>Nome do plugin registrado.</returns>
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

    /// <summary>
    /// Invoca o prompt Handlebars (síncrono e streaming) no kernel.
    /// </summary>
    /// <param name="template">Template Handlebars.</param>
    /// <param name="arguments">Argumentos do prompt.</param>
    /// <param name="promptTemplateFactory">Fábrica de templates Handlebars.</param>
    /// <returns>Fluxo assíncrono de conteúdo streaming.</returns>
    private async Task<IAsyncEnumerable<StreamingKernelContent>> InvokePrompt(string template, KernelArguments arguments, HandlebarsPromptTemplateFactory promptTemplateFactory)
    {
        var resultKernel = await _kernel.InvokePromptAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);
        _logger.Information("InvokePrompt  - InvokePromptAsync: {TemplateResult}", resultKernel);
        return _kernel.InvokePromptStreamingAsync(template, arguments, templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat, promptTemplateFactory: promptTemplateFactory);
    }

    /// <summary>
    /// Monta os argumentos do prompt (query e results).
    /// </summary>
    /// <param name="searchQuery">Consulta textual.</param>
    /// <param name="searchResult">Resultados vetoriais.</param>
    /// <returns>Argumentos do kernel.</returns>
    private static KernelArguments CreateArguments(string searchQuery, VectorSearchResult<TVector>[] searchResult) =>
        new KernelArguments
        {
            { "query", searchQuery },
            { "results", searchResult }
        };

    /// <summary>
    /// Renderiza o prompt Handlebars para diagnóstico/log.
    /// </summary>
    /// <param name="searchQuery">Consulta textual.</param>
    /// <param name="template">Template Handlebars.</param>
    /// <param name="results">Resultados vetoriais.</param>
    /// <param name="promptTemplateFactory">Fábrica de templates.</param>
    /// <returns>Prompt renderizado.</returns>
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
        _logger.Information("Rendered Prompt: {TemplateResult}", templateResult);
        return templateResult;
    }

    /// <summary>
    /// Obtém os top resultados vetoriais a partir do embedding da consulta.
    /// </summary>
    /// <param name="collection">Coleção tipada já carregada.</param>
    /// <param name="searchEmbedding">Embedding da consulta.</param>
    /// <returns>Array de resultados de busca vetorial.</returns>
    private static async Task<VectorSearchResult<TVector>[]> GetVectorsResults(
        VectorStoreCollection<ulong, TVector> collection,
        float[] searchEmbedding)
    {
        var searchEmbeddingCriteria = EmbeddingHelper.ConvertToReadOnlyMemory(searchEmbedding);
        var searchResult = collection.SearchAsync(searchEmbeddingCriteria, top: 2, new VectorSearchOptions<TVector>
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

    /// <summary>
    /// Remove um registro da coleção pela chave.
    /// </summary>
    /// <param name="nameCollection">Nome da coleção.</param>
    /// <param name="dataKey">Chave do registro.</param>
    /// <returns>Tarefa que conclui quando a exclusão for finalizada.</returns>
    public async Task DeleteAsync(string nameCollection, long dataKey)
    {
        var collection = await LoadCollection(nameCollection);
        await collection.DeleteAsync((ulong)dataKey);
    }
}
#endif
