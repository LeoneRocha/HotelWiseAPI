using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;
using HotelWise.Core.SDK.AI.Services;
using HotelWise.Core.SDK.AI.Configure;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Constants;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Core.SDK.Infrastructure.Middleware;
using HotelWise.Core.SDK.Security;
using HotelWise.Core.SDK.Services;
using HotelWise.Core.SDK.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Tests.Consolidation;

public class ConsolidationCoverageTests
{
    private static readonly int[] FilterItemsSource = [1, 2, 3, 4];
    private static readonly int[] FilterItemsExclude = [2, 4];
    private static readonly float[] EmbeddingSample = [1f];

    [Fact]
    public void ConfigurationAppSettingsHelper_Should_Read_Sections()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DBConnectionMySQL"] = "Server=localhost",
                ["TokenConfigurations:Secret"] = "s",
                ["AzureAd:TenantId"] = "t",
                ["Rag:BuildCollection"] = "true",
                ["MyKey"] = "val"
            })
            .Build();

        ConfigurationAppSettingsHelper.GetConnectionStringMySQL(config).Should().Be("Server=localhost");
        ConfigurationAppSettingsHelper.GetTokenConfigurations(config)["Secret"].Should().Be("s");
        ConfigurationAppSettingsHelper.GetAzureAdConfig(config)["TenantId"].Should().Be("t");
        ConfigurationAppSettingsHelper.GetRagConfig(config)["BuildCollection"].Should().Be("true");
        ConfigurationAppSettingsHelper.GetValueStringConfiguration(config, "MyKey").Should().Be("val");

        var act = () => ConfigurationAppSettingsHelper.GetSectionApp(null, "x");
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CultureDateTimeHelper_Should_Expose_Cultures_And_Brazil()
    {
        CultureDateTimeHelper.GetCultures().Should().Contain(c => c.Id == "pt-BR");
        CultureDateTimeHelper.GetTimeZonesIds().Should().NotBeEmpty();
        CultureDateTimeHelper.GetCultureBrazil().Should().Be("pt-BR");
        CultureDateTimeHelper.GetTimeZoneBrazil().Should().NotBeNullOrWhiteSpace();
        CultureDateTimeHelper.GetNameAndCulture("k").Should().Be("k");
        CultureDateTimeHelper.TranslateCulture([new CultureDisplayDto { Id = "en-US", Name = "English" }])
            .Should().ContainSingle(c => c.Name == "en-US");
    }

    [Fact]
    public void Dtos_And_Constants_Should_Construct()
    {
        new CultureDisplayDto { Id = "a", Name = "A" }.Name.Should().Be("A");
        new TimeZoneDisplayDto { Id = "z", Name = "Z" }.Id.Should().Be("z");
        new RepositoryInfo().ImplementationType.Should().BeNull();
        new SecurityDto { Id = "1", Name = "n", Role = "r" }.Name.Should().Be("n");
        new AskAssistantResponse { Message = "hi", Role = RoleAiPromptsType.Assistant }.Message.Should().Be("hi");
        new AskAssistantRequest { Message = "q" }.Role.Should().Be(RoleAiPromptsType.User);
        HelperCharSet.DefaultCharSet.Should().Be("latin1");
        AppConfigConstants.ConfigurationConfigurationNotBeNull.Should().NotBeNullOrWhiteSpace();
        ValidatorConstants.ValidateSuccessMessage_Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ServiceCollectionHelper_And_Di_Extensions_Should_Work()
    {
        var items = FilterItemsSource;
        ServiceCollectionHelper.FilterItems(items, FilterItemsExclude).Should().Equal(1, 3);

        var services = new ServiceCollection();
        services.AddScoped<object>();
        ServiceCollectionHelper.GetRegisteredInterfaces(services).Should().Contain(typeof(object));

        ConfigureServicesAI.RegisterGenericAiServices(services);
        services.Should().Contain(d => d.ServiceType == typeof(IAIInferenceAdapterFactory));

        ServiceCollectionConfigureCors.Configure(services);
        ServiceCollectionConfigureAppSettings.AddAndReturnTokenConfiguration(services, new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TokenConfigurations:Audience"] = "a",
                ["TokenConfigurations:Issuer"] = "i",
                ["TokenConfigurations:Secret"] = new string('x', 64),
                ["TokenConfigurations:Minutes"] = "30",
                ["TokenConfigurations:DaysToExpiry"] = "7"
            }).Build());

        ServiceCollectionConfigureAppSettings.AddAndReturnAzureAdConfig(services, new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AzureAd:TenantId"] = "tid",
                ["AzureAd:ClientId"] = "cid"
            }).Build()).TenantId.Should().Be("tid");
    }

    [Fact]
    public void ChatSession_And_TokenCounter_Helpers_Should_Work()
    {
        var history = new[]
        {
            new PromptMessageVO { RoleType = RoleAiPromptsType.User, Content = "Hi" },
            new PromptMessageVO { RoleType = RoleAiPromptsType.Assistant, Content = "Hello" }
        };
        ChatSessionHelper.GenerateContextMessage(history).Should().Contain("User: Hi");
        ChatSessionHelper.GetHistoryContext(history).Should().Contain("Hi");
        ChatSessionHelper.GetHistoryContext(Array.Empty<PromptMessageVO>()).Should().BeEmpty();

        TokenCounterHelper.CalculateTotalTokens(history).Should().BeGreaterThan(0);
        TokenCounterHelper.CalculateTotalDataVectorLength(new[]
        {
            new PromptMessageVO { DataContextRag = new[] { new DataVectorVO { DataVector = "abcd" } } }
        }).Should().Be(4);
    }

    [Fact]
    public void RagConfig_And_AiConfigs_Should_Expose_Defaults()
    {
        var rag = new RagConfig();
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.SemanticKernel);
        rag.AIChatServiceAdapter = AIChatServiceType.OllamaAdapter;
        rag.GetAInferenceAdapterType().Should().Be(InferenceAiAdapterType.Ollama);

        GroqApiConfig.ConfigSectionName.Should().Be("GroqApi");
        new OllamaConfig { NumPredict = 10 }.NumPredict.Should().Be(10);
        new AzureOpenAIConfig { ChatDeploymentName = "d" }.ChatDeploymentName.Should().Be("d");
        new SearchSettings { DelayBeforeSearchMilliseconds = 5 }.DelayBeforeSearchMilliseconds.Should().Be(5);
        new AzureAdConfig { TenantId = "t" }.TenantId.Should().Be("t");
        QdrantConfig.ConfigSectionName.Should().Be("Qdrant");
        RedisConfig.ConfigSectionName.Should().Be("Redis");
    }

    [Fact]
    public async Task AIInferenceService_Should_Delegate_To_Factory()
    {
        var adapter = new Mock<IAIInferenceAdapter>();
        adapter.Setup(a => a.GenerateChatCompletionAsync(It.IsAny<PromptMessageVO[]>())).ReturnsAsync("ok");
        adapter.Setup(a => a.GenerateChatCompletionByAgentAsync(It.IsAny<PromptMessageVO[]>())).ReturnsAsync("agent");
        adapter.Setup(a => a.GenerateChatCompletionByAgentSimpleRagAsync(It.IsAny<PromptMessageVO[]>())).ReturnsAsync("rag");
        adapter.Setup(a => a.GenerateEmbeddingAsync("t")).ReturnsAsync(EmbeddingSample);

        var factory = new Mock<IAIInferenceAdapterFactory>();
        factory.Setup(f => f.CreateAdapter(InferenceAiAdapterType.GroqApi)).Returns(adapter.Object);

        var svc = new AIInferenceService(Mock.Of<IConfiguration>(), factory.Object);
        var msgs = new[] { new PromptMessageVO { Content = "x", RoleType = RoleAiPromptsType.User } };

        (await svc.GenerateChatCompletionAsync(msgs, InferenceAiAdapterType.GroqApi)).Should().Be("ok");
        (await svc.GenerateChatCompletionByAgentAsync(msgs, InferenceAiAdapterType.GroqApi)).Should().Be("agent");
        (await svc.GenerateChatCompletionByAgentSimpleRagAsync(msgs, InferenceAiAdapterType.GroqApi)).Should().Be("rag");
        (await svc.GenerateEmbeddingAsync("t", InferenceAiAdapterType.GroqApi)).Should().Equal(1f);
    }

    [Fact]
    public void GenericVectorStoreServiceBase_Should_Set_UserId()
    {
        var sut = new TestVectorStoreService(Mock.Of<IMapper>(), Mock.Of<Serilog.ILogger>());
        sut.SetUserId(9);
        sut.ExposedUserId.Should().Be(9);
    }

    private sealed class TestVectorStoreService : GenericVectorStoreServiceBase
    {
        public TestVectorStoreService(IMapper mapper, Serilog.ILogger logger) : base(mapper, logger) { }
        public long ExposedUserId
        {
            get
            {
                var prop = typeof(GenericVectorStoreServiceBase).GetProperty("UserId",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                return (long)prop!.GetValue(this)!;
            }
        }
    }

    [Fact]
    public async Task GenericEntityServiceBase_Additional_Paths_Should_Work()
    {
        var repo = new Mock<IGenericRepository<SampleEnt>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<Serilog.ILogger>();
        var validator = new Mock<IValidator<SampleEnt>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<SampleEnt>(), default)).ReturnsAsync(new ValidationResult());

        var entity = new SampleEnt { Id = 1, Name = "n" };
        var dto = new SampleDto2 { Id = 1, Name = "n" };
        mapper.Setup(m => m.Map<SampleEnt>(dto)).Returns(entity);
        mapper.Setup(m => m.Map<SampleDto2>(entity)).Returns(dto);
        mapper.Setup(m => m.Map<List<SampleDto2>>(It.IsAny<List<SampleEnt>>())).Returns(new List<SampleDto2> { dto });
        mapper.Setup(m => m.Map<Expression<Func<SampleEnt, bool>>>(It.IsAny<Expression<Func<SampleDto2, bool>>>()))
            .Returns((Expression<Func<SampleEnt, bool>>)(e => true));
        mapper.Setup(m => m.Map<IEnumerable<SampleEnt>>(It.IsAny<IEnumerable<SampleDto2>>())).Returns(new[] { entity });

        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<SampleEnt, bool>>>())).ReturnsAsync(new List<SampleEnt> { entity });
        repo.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(entity);
        repo.Setup(r => r.FetchAsync(0, 1)).ReturnsAsync(new List<SampleEnt> { entity });
        repo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        repo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<SampleEnt>>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.UpdateRangeAsync(It.IsAny<IEnumerable<SampleEnt>>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.CountAsync()).ReturnsAsync(1);

        var sut = new SampleSvc(repo.Object, mapper.Object, logger.Object, validator.Object);
        sut.SetUserId(1);
        (await sut.GetByIdAsync(1))!.Name.Should().Be("n");
        (await sut.FindAsync(d => true)).Should().HaveCount(1);
        (await sut.UpdateAsync(dto)).Success.Should().BeTrue();
        await sut.DeleteAsync(1);
        (await sut.FetchAsync(0, 1)).Should().HaveCount(1);
        await sut.AddRangeAsync(new[] { dto });
        await sut.UpdateRangeAsync(new[] { dto });
        (await sut.CountAsync()).Should().Be(1);
    }

    public class SampleEnt { public long Id { get; set; } public string Name { get; set; } = ""; }
    public class SampleDto2 { public long Id { get; set; } public string Name { get; set; } = ""; }
    private sealed class SampleSvc : GenericEntityServiceBase<SampleEnt, SampleDto2>
    {
        public SampleSvc(IGenericRepository<SampleEnt> r, IMapper m, Serilog.ILogger l, IValidator<SampleEnt> v)
            : base(r, m, l, v) { }
    }

    [Fact]
    public async Task RequestLoggingMiddleware_Should_Invoke_Next()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var logger = new Mock<Serilog.ILogger>();
        logger.Setup(l => l.Information(It.IsAny<string>(), It.IsAny<object[]>()));
        var invoked = false;
        var mw = new RequestLoggingMiddleware(_ => { invoked = true; return Task.CompletedTask; }, logger.Object);
        await mw.InvokeAsync(context);
        invoked.Should().BeTrue();
    }

    [Fact]
    public void TokenService_GetPrincipal_Should_Validate_Alg()
    {
        var config = new Mock<ITokenConfigurationDto>();
        config.SetupGet(c => c.Secret).Returns(new string('k', 64));
        config.SetupGet(c => c.Issuer).Returns("iss");
        config.SetupGet(c => c.Audience).Returns("aud");
        config.SetupGet(c => c.Minutes).Returns(5);
        var svc = new TokenService(config.Object);
        var token = svc.GenerateAccessToken([new Claim(ClaimTypes.Name, "u")]);
        var principal = svc.GetPrincipalFromExpiredToken(token);
        principal.Identity!.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void DataHelper_Additional_Methods_Should_Work()
    {
        DataHelper.GetDateTimeCustomFormat(new DateTime(2020, 1, 2, 3, 4, 5)).Should().Contain("2020");
        DataHelper.GetDateTimeNow().Kind.Should().Be(DateTimeKind.Utc);
        DataHelper.SetCulture();
    }
}
