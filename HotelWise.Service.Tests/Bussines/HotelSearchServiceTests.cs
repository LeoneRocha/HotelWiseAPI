using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Bussines;
using HotelWise.Service.Entity;
using Serilog;

namespace HotelWise.Service.Tests.Bussines;

public class HotelSearchServiceTests
{
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IApplicationIAConfig> _configMock = new();
    private readonly Mock<IHotelRepository> _hotelRepoMock = new();
    private readonly Mock<IVectorStoreService<HotelVector>> _vectorStoreMock = new();
    private readonly Mock<IValidator<Hotel>> _validatorMock = new();
    private readonly Mock<IAIInferenceService> _inferenceMock = new();

    public HotelSearchServiceTests()
    {
        _configMock.SetupGet(c => c.RagConfig).Returns(new RagConfig
        {
            AIChatServiceAdapter = AIChatServiceType.SemanticKernel
        });
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private HotelSearchService CreateSut()
    {
        return new HotelSearchService(
            _loggerMock.Object,
            _mapperMock.Object,
            _configMock.Object,
            _hotelRepoMock.Object,
            _vectorStoreMock.Object,
            _validatorMock.Object,
            _inferenceMock.Object);
    }

    [Fact]
    public async Task SemanticSearch_WithValidCriteria_ShouldReturnEnrichedAndFilteredHotels()
    {
        // Arrange
        var sut = CreateSut();
        var criteria = new SearchCriteria { SearchTextCriteria = "Hotel com piscina em Copacabana" };

        var hotelEntities = new[]
        {
            new Hotel { HotelId = 1, HotelName = "Copacabana Palace", City = "Rio de Janeiro", InitialRoomPrice = 1200m, Stars = 5, StateCode = "RJ", Tags = ["luxo", "praia"] },
            new Hotel { HotelId = 2, HotelName = "Ipanema Inn", City = "Rio de Janeiro", InitialRoomPrice = 600m, Stars = 4, StateCode = "RJ", Tags = ["praia"] }
        };

        var hotelDtos = new[]
        {
            new HotelDto { HotelId = 1, HotelName = "Copacabana Palace", City = "Rio de Janeiro", InitialRoomPrice = 1200m, Stars = 5, StateCode = "RJ", Tags = ["luxo", "praia"] },
            new HotelDto { HotelId = 2, HotelName = "Ipanema Inn", City = "Rio de Janeiro", InitialRoomPrice = 600m, Stars = 4, StateCode = "RJ", Tags = ["praia"] }
        };

        var hotelVectors = new[]
        {
            new HotelVector { DataKey = 1, HotelName = "Copacabana Palace", Description = "Frente ao mar", Score = 0.95 },
            new HotelVector { DataKey = 2, HotelName = "Ipanema Inn", Description = "Proximo a praia", Score = 0.85 }
        };

        _hotelRepoMock.Setup(r => r.GetTotalHotelsCountAsync()).ReturnsAsync(2);
        _hotelRepoMock.Setup(r => r.FetchHotelsAsync(0, 10)).ReturnsAsync(hotelEntities);
        _mapperMock.Setup(m => m.Map<HotelDto[]>(It.IsAny<Hotel[]>())).Returns(hotelDtos);

        _vectorStoreMock.Setup(v => v.VectorizedSearchAsync(criteria))
            .ReturnsAsync(new ServiceResponse<HotelVector[]>
            {
                Success = true,
                Data = hotelVectors
            });

        const string aiResponse = """
            Encontrei excelentes opções para sua estadia:
            ### Hotel Copacabana Palace
            <!-- ID-Hotel: 1 -->
            Excelente opção de luxo frente ao mar.
            """;

        _inferenceMock.Setup(i => i.GenerateChatCompletionByAgentSimpleRagAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(aiResponse);

        // Act
        var result = await sut.SemanticSearch(criteria);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.HotelsVectorResult.Should().HaveCount(1);
        result.Data.HotelsVectorResult[0].HotelId.Should().Be(1);
        result.Data.HotelsVectorResult[0].HotelName.Should().Be("Copacabana Palace");
        result.Data.PromptResultContent.Should().Contain("Copacabana Palace");
    }

    [Fact]
    public async Task SemanticSearch_WhenFetchHotelsThrows_ShouldHandleGracefully()
    {
        // Arrange
        var sut = CreateSut();
        var criteria = new SearchCriteria { SearchTextCriteria = "Busca" };

        _hotelRepoMock.Setup(r => r.GetTotalHotelsCountAsync()).ThrowsAsync(new Exception("Database connection failure"));

        _vectorStoreMock.Setup(v => v.VectorizedSearchAsync(criteria))
            .ReturnsAsync(new ServiceResponse<HotelVector[]>
            {
                Success = true,
                Data = []
            });

        _inferenceMock.Setup(i => i.GenerateChatCompletionByAgentSimpleRagAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync("Nenhum resultado");

        // Act
        var result = await sut.SemanticSearch(criteria);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task SemanticSearch_WhenExceptionOccurs_ShouldHandleGracefully()
    {
        // Arrange
        var sut = CreateSut();
        var criteria = new SearchCriteria { SearchTextCriteria = "Busca que falha" };

        _hotelRepoMock.Setup(r => r.GetTotalHotelsCountAsync()).ReturnsAsync(1);
        _hotelRepoMock.Setup(r => r.FetchHotelsAsync(0, 10)).ReturnsAsync([new Hotel { HotelId = 1 }]);
        _mapperMock.Setup(m => m.Map<HotelDto[]>(It.IsAny<Hotel[]>())).Returns([new HotelDto { HotelId = 1 }]);

        _vectorStoreMock.Setup(v => v.VectorizedSearchAsync(criteria))
            .ThrowsAsync(new Exception("Vector store connection error"));

        // Act
        var result = await sut.SemanticSearch(criteria);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Data.Should().NotBeNull();
        result.Data!.HotelsVectorResult.Should().BeEmpty();
        result.Data.HotelsIAResult.Should().BeEmpty();
    }

    [Fact]
    public void FilterHotelsByIAResult_WithNullParameters_ShouldThrowException()
    {
        Action act = () => HotelSearchService.FilterHotelsByIAResult(null!, []);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*não podem ser nulos*");
    }
}

