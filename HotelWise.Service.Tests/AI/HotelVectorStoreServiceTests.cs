using AutoMapper;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Service.AI;

namespace HotelWise.Service.Tests.AI;

public class HotelVectorStoreServiceTests
{
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IApplicationIAConfig> _appConfig = new();
    private readonly Mock<IVectorStoreAdapterFactory> _adapterFactory = new();
    private readonly Mock<IVectorStoreAdapter<HotelVector>> _adapter = new();
    private readonly Mock<IAIInferenceService> _inference = new();

    public HotelVectorStoreServiceTests()
    {
        _appConfig.SetupGet(c => c.RagConfig).Returns(new RagConfig
        {
            VectorStoreCollectionPrefixName = "test_",
            AIChatServiceAdapter = AIChatServiceType.SemanticKernel
        });
        _adapterFactory.Setup(f => f.CreateAdapter<HotelVector>()).Returns(_adapter.Object);
    }

    private HotelVectorStoreService CreateSut() =>
        new(
            _logger.Object,
            _mapper.Object,
            _appConfig.Object,
            _adapterFactory.Object,
            _inference.Object);

    [Fact]
    public async Task GetById_Should_Return_Vector_From_Adapter()
    {
        var expected = new HotelVector
        {
            DataKey = 42,
            HotelName = "Vector Hotel",
            Description = "Desc"
        };
        _adapter.Setup(a => a.GetByKey("test_skhotels", 42UL)).ReturnsAsync(expected);

        var result = await CreateSut().GetById(42);

        result.Should().NotBeNull();
        result!.HotelName.Should().Be("Vector Hotel");
        result.DataKey.Should().Be(42UL);
    }

    [Fact]
    public async Task GetById_Should_Return_Null_When_Adapter_Throws()
    {
        _adapter.Setup(a => a.GetByKey(It.IsAny<string>(), It.IsAny<ulong>()))
            .ThrowsAsync(new InvalidOperationException("store offline"));

        var result = await CreateSut().GetById(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GenerateEmbeddingAsync_Should_Call_InferenceService()
    {
        _inference.Setup(i => i.GenerateEmbeddingAsync("Texto", InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync([0.1f, 0.2f]);

        var result = await CreateSut().GenerateEmbeddingAsync("Texto");

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpsertDataAsync_Should_GenerateEmbedding_And_Call_Adapter()
    {
        var vector = new HotelVector { DataKey = 10, Description = "Hotel com vista para o mar" };
        _inference.Setup(i => i.GenerateEmbeddingAsync(vector.Description, InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync([0.5f, 0.6f]);
        _adapter.Setup(a => a.UpsertDataAsync("test_skhotels", vector)).Returns(Task.CompletedTask);

        var sut = CreateSut();
        await sut.UpsertDataAsync(vector);

        _adapter.Verify(a => a.UpsertDataAsync("test_skhotels", vector), Times.Once);
    }

    [Fact]
    public async Task UpsertDatasAsync_Should_Upsert_NonExisting_Vectors()
    {
        var vectors = new[]
        {
            new HotelVector { DataKey = 1, Description = "Hotel 1" },
            new HotelVector { DataKey = 2, Description = "Hotel 2" }
        };

        _adapter.Setup(a => a.Exists("test_skhotels", 1UL)).ReturnsAsync(false);
        _adapter.Setup(a => a.Exists("test_skhotels", 2UL)).ReturnsAsync(true);

        _inference.Setup(i => i.GenerateEmbeddingAsync("Hotel 1", InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync([0.1f, 0.2f]);
        _adapter.Setup(a => a.UpsertDatasAsync("test_skhotels", It.IsAny<HotelVector[]>())).Returns(Task.CompletedTask);

        var sut = CreateSut();
        await sut.UpsertDatasAsync(vectors);

        _adapter.Verify(a => a.UpsertDatasAsync("test_skhotels", It.Is<HotelVector[]>(arr => arr.Length == 1)), Times.Once);
    }

    [Fact]
    public async Task VectorizedSearchAsync_Should_Return_Adapter_Results()
    {
        var criteria = new SearchCriteria { SearchTextCriteria = "praia" };
        float[] embedding = [0.1f, 0.2f];
        HotelVector[] vectors =
        [
            new HotelVector { DataKey = 1, HotelName = "Beach", Description = "Near sea", Score = 0.9 }
        ];

        _inference.Setup(i => i.GenerateEmbeddingAsync("praia", InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(embedding);
        _adapter.Setup(a => a.VectorizedSearchAsync("test_skhotels", embedding, criteria))
            .ReturnsAsync(vectors);

        var response = await CreateSut().VectorizedSearchAsync(criteria);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(v => v.HotelName == "Beach");
    }

    [Fact]
    public async Task VectorizedSearchAsync_WhenExceptionOccurs_ShouldReturnError()
    {
        var criteria = new SearchCriteria { SearchTextCriteria = "praia" };
        _inference.Setup(i => i.GenerateEmbeddingAsync(It.IsAny<string>(), It.IsAny<InferenceAiAdapterType>()))
            .ThrowsAsync(new Exception("Falha de conexão"));

        var response = await CreateSut().VectorizedSearchAsync(criteria);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Falha de conexão");
    }

    [Fact]
    public async Task SearchAndAnalyzePluginAsync_Should_Return_Results()
    {
        float[] embedding = [0.1f, 0.2f];
        HotelVector[] vectors = [new HotelVector { DataKey = 1, HotelName = "Pousada" }];

        _inference.Setup(i => i.GenerateEmbeddingAsync("pousada", InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(embedding);
        _adapter.Setup(a => a.SearchAndAnalyzePluginAsync("test_skhotels", "pousada", embedding))
            .ReturnsAsync(vectors);

        var response = await CreateSut().SearchAndAnalyzePluginAsync("pousada");

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task SearchAndAnalyzePluginAsync_WhenExceptionOccurs_ShouldReturnError()
    {
        _inference.Setup(i => i.GenerateEmbeddingAsync(It.IsAny<string>(), It.IsAny<InferenceAiAdapterType>()))
            .ThrowsAsync(new Exception("Erro de inferência"));

        var response = await CreateSut().SearchAndAnalyzePluginAsync("pousada");

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Erro de inferência");
    }

    [Fact]
    public async Task DeleteAsync_Should_Call_Adapter()
    {
        _adapter.Setup(a => a.DeleteAsync("test_skhotels", 50L)).Returns(Task.CompletedTask);

        var sut = CreateSut();
        await sut.DeleteAsync(50);

        _adapter.Verify(a => a.DeleteAsync("test_skhotels", 50L), Times.Once);
    }
}

