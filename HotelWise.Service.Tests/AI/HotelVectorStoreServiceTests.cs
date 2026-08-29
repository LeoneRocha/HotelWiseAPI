using AutoMapper;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
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
}
