using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Bussines;

public class HotelSearchServiceTests
{
    private readonly Mock<Serilog.ILogger> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IApplicationIAConfig> _configMock = new();
    private readonly Mock<IHotelRepository> _hotelRepoMock = new();
    private readonly Mock<IVectorStoreService<HotelVector>> _vectorStoreMock = new();
    private readonly Mock<IValidator<Hotel>> _validatorMock = new();
    private readonly Mock<IAIInferenceService> _aiInferenceMock = new();

    public HotelSearchServiceTests()
    {
        _configMock.SetupGet(c => c.RagConfig).Returns(new RagConfig
        {
            AIChatServiceAdapter = AIChatServiceType.GroqApi
        });

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private HotelSearchService CreateSut() =>
        new(
            _loggerMock.Object,
            _mapperMock.Object,
            _configMock.Object,
            _hotelRepoMock.Object,
            _vectorStoreMock.Object,
            _validatorMock.Object,
            _aiInferenceMock.Object
        );

    // Cenário: Chamada a SemanticSearch com critério de busca vazio.
    // Objetivo: Deve retornar Success = false imediatamente sem chamar banco ou IA.
    [Fact]
    public async Task SemanticSearch_WithEmptySearchCriteria_ShouldReturnFailure()
    {
        // Arrange
        var sut = CreateSut();
        var criteria = new SearchCriteria { SearchTextCriteria = "" };

        // Act
        var result = await sut.SemanticSearch(criteria);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().NotBeNull();
        result.Data.HotelsVectorResult.Should().BeEmpty();
    }

    // Cenário: Fluxo de sucesso completo de SemanticSearch.
    // Objetivo: Deve buscar hotéis do DB, realizar busca vetorial, gerar prompts, chamar IA e filtrar resultados.
    [Fact]
    public async Task SemanticSearch_WithValidCriteria_ShouldReturnFilteredHotels()
    {
        // Arrange
        var sut = CreateSut();
        var criteria = new SearchCriteria { SearchTextCriteria = "Hotel com piscina no Rio de Janeiro" };

        var hotelEntities = new[]
        {
            new Hotel { HotelId = 1, HotelName = "Hotel Copacabana", Description = "Beira mar com piscina", City = "Rio de Janeiro", InitialRoomPrice = 300 },
            new Hotel { HotelId = 2, HotelName = "Hotel Ipanema", Description = "Proximo a praia", City = "Rio de Janeiro", InitialRoomPrice = 400 }
        };

        var hotelDtos = new[]
        {
            new HotelDto { HotelId = 1, HotelName = "Hotel Copacabana", Description = "Beira mar com piscina", City = "Rio de Janeiro", InitialRoomPrice = 300 },
            new HotelDto { HotelId = 2, HotelName = "Hotel Ipanema", Description = "Proximo a praia", City = "Rio de Janeiro", InitialRoomPrice = 400 }
        };

        _hotelRepoMock.Setup(r => r.GetTotalHotelsCountAsync()).ReturnsAsync(2);
        _hotelRepoMock.Setup(r => r.FetchHotelsAsync(0, 10)).ReturnsAsync(hotelEntities);
        _mapperMock.Setup(m => m.Map<HotelDto[]>(hotelEntities)).Returns(hotelDtos);

        var vectorResults = new[]
        {
            new HotelVector { DataKey = 1, HotelName = "Hotel Copacabana", Description = "Beira mar com piscina", Score = 0.95 },
            new HotelVector { DataKey = 2, HotelName = "Hotel Ipanema", Description = "Proximo a praia", Score = 0.85 }
        };

        _vectorStoreMock.Setup(v => v.VectorizedSearchAsync(criteria))
            .ReturnsAsync(new ServiceResponse<HotelVector[]>
            {
                Success = true,
                Data = vectorResults
            });

        // IA retorna resposta contendo o ID oculto no padrão <!-- ID-Hotel: 1 -->
        _aiInferenceMock.Setup(ai => ai.GenerateChatCompletionByAgentSimpleRagAsync(It.IsAny<PromptMessageVO[]>(), It.IsAny<InferenceAiAdapterType>()))
            .ReturnsAsync("Recomendo este hotel excelente: <!-- ID-Hotel: 1 --> Hotel Copacabana");

        // Act
        var result = await sut.SemanticSearch(criteria);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.PromptResultContent.Should().Contain("Hotel Copacabana");
        result.Data.HotelsVectorResult.Should().HaveCount(1);
        result.Data.HotelsVectorResult[0].HotelId.Should().Be(1);
    }

    // Cenário: Exceção no fluxo de inferência ou busca vetorial.
    // Objetivo: Deve capturar erro, logar e retornar Success = false com lista vazia.
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
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Data.HotelsVectorResult.Should().BeEmpty();
        result.Data.HotelsIAResult.Should().BeEmpty();
    }
}
