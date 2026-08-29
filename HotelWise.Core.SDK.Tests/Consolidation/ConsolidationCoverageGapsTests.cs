using System.Diagnostics;
using FluentValidation.Results;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;
using HotelWise.Core.SDK.AI.Services;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Constants;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Core.SDK.Infrastructure.Middleware;
using HotelWise.Core.SDK.Logging;
using HotelWise.Core.SDK.Security;
using HotelWise.Core.SDK.Validation;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;

namespace HotelWise.Core.SDK.Tests.Consolidation;

public class ConsolidationCoverageGapsTests
{
    private static readonly float[] SingleFloatEmbedding = [1f];

    [Fact]
    public void LogAppHelper_Should_Cover_Logging_And_Version_Paths()
    {
        var sw = Stopwatch.StartNew();
        sw.Stop();
        LogAppHelper.GetDurationStopwatch(sw).Should().MatchRegex(@"\d{2}:\d{2}:\d{2}");

        var logger = new Mock<Serilog.ILogger>();
        LogAppHelper.LogException(logger.Object, new AppWarningException("warn"), "API");
        LogAppHelper.LogException(logger.Object, new InvalidOperationException("err"), "API");
        logger.Verify(l => l.Warning(It.IsAny<string>()), Times.AtLeastOnce);
        logger.Verify(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.AtLeastOnce);

        var info = LogAppHelper.GetInformationVersionProduct();
        info.Should().NotBeNull();
        info.Message.Should().NotBeNullOrEmpty();
        LogAppHelper.ShowInformationVersionProductString().Should().NotBeNullOrEmpty();
        LogAppHelper.PrintLogInformationVersionProduct(logger.Object);

        var cfg = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["APP_ENVIRONMENT"] = "Development",
                ["Serilog:MinimumLevel:Default"] = "Information"
            })
            .Build();
        LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(cfg);
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Should().Be("Development");

        using var serilog = LogAppHelper.CreateLogger(cfg);
        serilog.Should().NotBeNull();
    }

    [Fact]
    public void SecurityHelper_Should_Create_Token_And_Validate_Base64()
    {
        var token = SecurityHelper.CreateToken(new SecurityDto
        {
            Id = "7",
            Name = "user",
            Role = "Admin",
            SecurityKeyConfig = new string('k', 64)
        });
        token.Should().NotBeNullOrWhiteSpace();

        SecurityHelper.IsBase64String("").Should().BeFalse();
        SecurityHelper.IsBase64String("!!!").Should().BeFalse();
        SecurityHelper.IsBase64String(Convert.ToBase64String(new byte[] { 1, 2, 3 })).Should().BeTrue();
    }

    [Fact]
    public void ServiceCollectionHelper_Should_Discover_And_Register_Interfaces()
    {
        var assemblies = new[] { typeof(ConsolidationCoverageGapsTests).Assembly };
        var infos = ServiceCollectionHelper.GetInterfaces(["CoverageSvc"], assemblies);
        infos.Should().Contain(i => i.InterfaceType == typeof(IGapCoverageSvc));

        var services = new ServiceCollection();
        ServiceCollectionHelper.RegisterInterfaces(
            services,
            ["CoverageSvc"],
            new List<Type>(),
            assemblies);
        services.Should().Contain(d => d.ServiceType == typeof(IGapCoverageSvc)
            && d.ImplementationType == typeof(GapCoverageSvc));
    }

    public interface IGapCoverageSvc { }
    public class GapCoverageSvc : IGapCoverageSvc { }

    [Fact]
    public void VectorStoreAdapterFactory_Should_Create_Adapter()
    {
        var factory = new VectorStoreAdapterFactory(
            Mock.Of<IApplicationIAConfig>(),
            Mock.Of<VectorStore>(),
            Kernel.CreateBuilder().Build(),
            Mock.Of<Serilog.ILogger>());

        var adapter = factory.CreateAdapter<GapDataVector>();
        adapter.Should().NotBeNull();
    }

    private sealed class GapDataVector : DataVectorBase { }

    [Fact]
    public void DataHelper_Brazil_And_TimeZone_Should_Work()
    {
        DataHelper.GetDateTimeNowBrazil().Kind.Should().Be(DateTimeKind.Unspecified);
        DataHelper.GetDateTimeNowToLog().Should().BeCloseTo(DataHelper.GetDateTimeNowBrazil(), TimeSpan.FromSeconds(2));
        DataHelper.GetDateTimeNowToProcess().Should().BeCloseTo(DataHelper.GetDateTimeNowBrazil(), TimeSpan.FromSeconds(2));
        DataHelper.GetDateTimeNowToPersistData().Should().BeCloseTo(DataHelper.GetDateTimeNowBrazil(), TimeSpan.FromSeconds(2));

        var utc = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        DataHelper.ApplyTimeZone(utc, "E. South America Standard Time")
            .Should().BeBefore(utc.AddHours(1));
    }

    [Fact]
    public void ServiceCollectionConfigureCors_Should_Build_Policies()
    {
        var services = new ServiceCollection();
        ServiceCollectionConfigureCors.Configure(services);
        var sp = services.BuildServiceProvider();
        var options = sp.GetRequiredService<IOptions<CorsOptions>>().Value;
        options.GetPolicy(options.DefaultPolicyName!).Should().NotBeNull();
        options.GetPolicy("AllowAnyOrigin").Should().NotBeNull();
    }

    [Fact]
    public void EntityTypeConfigurationConstants_Should_Map_All_Providers()
    {
        foreach (ETypeDataBase db in Enum.GetValues<ETypeDataBase>())
        {
            EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase(db).Should().BeGreaterThan(0);
            EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase(db).Should().NotBeNullOrWhiteSpace();
        }

        EntityTypeConfigurationConstants.Type_Varchar_255.Should().Contain("255");
        EntityTypeConfigurationConstants.Type_Varchar_40.Should().Contain("40");
        EntityTypeConfigurationConstants.Type_Varchar_20.Should().Contain("20");
        EntityTypeConfigurationConstants.Language_Default_PTBR.Should().Be("pt-BR");
        EntityTypeConfigurationConstants.ApplicationLanguage_ResourceKey_Default.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void RagConfig_And_AiConfigs_Should_Cover_Branches_And_Props()
    {
        var rag = new RagConfig
        {
            AIChatServiceApi = AIChatServiceType.GroqApi,
            AIEmbeddingServiceApi = AIEmbeddingServiceType.OpenAIEmbeddings,
            AIEmbeddingServiceAdapter = AIEmbeddingServiceType.DefaultEmbeddings,
            BuildCollection = false,
            VectorStoreCollectionPrefixName = "p",
            VectorStoreDimensions = 512,
            DataLoadingBatchSize = 3,
            DataLoadingBetweenBatchDelayInMilliseconds = 1,
            PdfFilePaths = ["a.pdf"],
            VectorStoreType = VectorStoreType.Qdrant,
            SearchSettings = new SearchSettings { DelayBeforeSearchMilliseconds = 2 }
        };

        rag.AIChatServiceAdapter = AIChatServiceType.Default;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.SemanticKernel);
        rag.AIChatServiceAdapter = AIChatServiceType.SemanticKernel;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.SemanticKernel);
        rag.AIChatServiceAdapter = AIChatServiceType.GroqApi;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.GroqApi);
        rag.AIChatServiceAdapter = AIChatServiceType.MistralApi;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.GroqApi);
        rag.AIChatServiceAdapter = AIChatServiceType.Ollama;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.Ollama);
        rag.AIChatServiceAdapter = AIChatServiceType.OllamaAdapter;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.Ollama);
        rag.AIChatServiceAdapter = (AIChatServiceType)999;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.SemanticKernel);

        AzureOpenAIConfig.ConfigSectionName.Should().Be("AzureOpenAI");
        AzureOpenAIEmbeddingsConfig.ConfigSectionName.Should().Be("AzureOpenAIEmbeddings");
        new AzureOpenAIEmbeddingsConfig { DeploymentName = "d" }.DeploymentName.Should().Be("d");
        OpenAIConfig.ConfigSectionName.Should().Be("OpenAI");
        OpenAIEmbeddingsConfig.ConfigSectionName.Should().Be("OpenAIEmbeddings");
        MistralApiConfig.ConfigSectionName.Should().Be("MistralApi");
        MistralApiEmbeddingsConfig.ConfigSectionName.Should().Be("MistralApiEmbeddings");
        AzureAISearchConfig.ConfigSectionName.Should().Be("AzureAISearch");
        WeaviateConfig.ConfigSectionName.Should().Be("Weaviate");
        new AzureCosmosDBConfig { ConnectionString = "c", DatabaseName = "d" }.DatabaseName.Should().Be("d");
        AzureCosmosDBConfig.MongoDBConfigSectionName.Should().NotBeNullOrWhiteSpace();
        AzureCosmosDBConfig.NoSQLConfigSectionName.Should().NotBeNullOrWhiteSpace();
        new QdrantConfig { Host = "h", Port = 1, Https = true, ApiKey = "k" }.Host.Should().Be("h");
        new RedisConfig { ConnectionConfiguration = "localhost" }.ConnectionConfiguration.Should().Be("localhost");
        new OllamaConfig { Temperature = 0.5f, TopP = 0.9f, Seed = 1 }.Temperature.Should().Be(0.5f);
        new AiInferenceConfigBaseProbe
        {
            Endpoint = "e",
            ApiKey = "a",
            ModelId = "m",
            OrgId = "o",
            EndpointEmbeddings = "ee",
            ModelIdEmbeddings = "me"
        }.Endpoint.Should().Be("e");
    }

    private sealed class AiInferenceConfigBaseProbe : AiInferenceConfigBase { }

    [Fact]
    public void Dtos_Exceptions_And_Helpers_Should_Cover_Remaining()
    {
        new AppInformationVersionProductDto
        {
            Id = "1",
            Name = "n",
            Version = "v",
            EnvironmentName = "e",
            Message = "m"
        }.Message.Should().Be("m");

        new AppWarningException().Should().BeAssignableTo<Exception>();
        new AppWarningException("m").Message.Should().Be("m");
        new AppWarningException("m", new Exception("i")).InnerException!.Message.Should().Be("i");

        var vec = new GapDataVector { DataKey = 1, Score = 0.5, Tags = ["t"] };
        vec.Embedding = new ReadOnlyMemory<float>(SingleFloatEmbedding);
        vec.Tags.Should().Contain("t");

        MarkdownHelper.HasMarkdown("").Should().BeFalse();
        MarkdownHelper.RemoveMarkdown("").Should().BeEmpty();
        MarkdownHelper.RemoveMarkdown("# Title\n- item\n1. n\n> q\n![i](u)\n[a](u)\n**x**")
            .Should().NotContain("**");
        MarkdownHelper.ConvertToHtmlIfMarkdown("plain").Should().Be("plain");
        MarkdownHelper.ConvertToHtmlIfMarkdown("**bold**").Should().Contain("strong");

        var tokenVo = new TokenVO(true, "c", "e", "a", "r");
        tokenVo.Authenticated.Should().BeTrue();
        tokenVo.AccessToken.Should().Be("a");
        new TokenVO().Authenticated.Should().BeFalse();

        TokenCounterHelper.CalculateDataVectorLength(null!).Should().Be(0);
        TokenCounterHelper.CalculateDataVectorLength([]).Should().Be(0);
        TokenCounterHelper.CalculateTotalDataVectorLength(null!).Should().Be(0);
        TokenCounterHelper.CalculateTotalTokens(null!).Should().Be(0);
        TokenCounterHelper.CountTokensFromPrompt(null!).Should().Be(0);

        var errors = HelperValidation.GetErrorsMap(new ValidationResult(new[]
        {
            new ValidationFailure("Name", "CODE_X|default msg"),
            new ValidationFailure("Age", "plain error")
        }));
        errors.Should().HaveCount(2);
        HelperValidation.GetErrorsMap(null).Should().BeEmpty();
        HelperValidation.GetErrorsMap(new ValidationResult()).Should().BeEmpty();
        HelperValidation.ConvertValidationFailureListToErroResponse(
            [new ValidationFailure("A", "a"), new ValidationFailure("A", "b")]).Should().HaveCount(1);

        var pm = new PromptMessageVO
        {
            Content = "abcd",
            RoleType = RoleAiPromptsType.System,
            AgentName = "agent",
            DataContextRag = [new DataVectorVO { KeyVector = "k", DataVector = "x" }]
        };
        pm.RoleType.Should().Be(RoleAiPromptsType.System);
        pm.Role.Should().NotBeNullOrWhiteSpace();
        pm.TokenCount.Should().BeGreaterThan(0);
        pm.ContentLenght.Should().Be(4);
        pm.AgentName.Should().Be("agent");
        new PromptMessageVO { DataContextRag = [new DataVectorVO { DataVector = "abcd" }] }.TokenCount.Should().Be(0);
    }

    [Fact]
    public async Task CorrelationIdMiddleware_Should_Populate_Items_And_Trace()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var mw = new CorrelationIdMiddleware(_ => Task.CompletedTask);
        await mw.InvokeAsync(context);
        context.Items[CorrelationIdMiddleware.ItemKey].Should().NotBeNull();
        context.TraceIdentifier.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenericRepositoryBase_CreateContext_Should_Work()
    {
        var options = new DbContextOptionsBuilder<GapDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var ctx = new GapDbContext(options);
        var repo = new GapRepo(ctx, options);
        using var created = repo.CreateContextPublic();
        created.Should().NotBeNull();
    }

    private sealed class GapDbContext : DbContext
    {
        public GapDbContext(DbContextOptions<GapDbContext> options) : base(options) { }
        public DbSet<GapEntity> Items => Set<GapEntity>();
    }

    private sealed class GapEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
    }

    private sealed class GapRepo : GenericRepositoryBase<GapEntity, GapDbContext>
    {
        public GapRepo(GapDbContext context, DbContextOptions<GapDbContext> options) : base(context, options) { }
        public GapDbContext CreateContextPublic() => CreateContext();
    }
}
