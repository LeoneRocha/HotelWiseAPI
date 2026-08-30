using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity.HotelServices;

namespace HotelWise.Service.Tests.Entity;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _hotelRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IGenerateHotelService> _generateHotelService = new();
    private readonly Mock<IVectorStoreService<HotelVector>> _vectorStore = new();
    private readonly Mock<IValidator<Hotel>> _validator = new();
    private readonly Mock<IApplicationIAConfig> _appConfig = new();

    public HotelServiceTests()
    {
        _appConfig.SetupGet(c => c.RagConfig).Returns(new RagConfig());
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private HotelService CreateSut() =>
        new(
            _logger.Object,
            _mapper.Object,
            _appConfig.Object,
            _hotelRepository.Object,
            _generateHotelService.Object,
            _vectorStore.Object,
            _validator.Object);

    [Fact]
    public async Task GetAllHotelsAsync_Should_Return_Mapped_Hotels_Ordered_By_Name()
    {
        var hotels = new List<Hotel>
        {
            new() { HotelId = 2, HotelName = "Zulu" },
            new() { HotelId = 1, HotelName = "Alpha" }
        };
        HotelDto[] dtos =
        [
            new HotelDto { HotelId = 2, HotelName = "Zulu" },
            new HotelDto { HotelId = 1, HotelName = "Alpha" }
        ];

        _hotelRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(hotels);
        _mapper.Setup(m => m.Map<HotelDto[]>(hotels)).Returns(dtos);

        var response = await CreateSut().GetAllHotelsAsync();

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Select(h => h.HotelName).Should().Equal("Alpha", "Zulu");
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllHotelsAsync_Should_Add_Error_When_Repository_Throws()
    {
        _hotelRepository.Setup(r => r.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("db down"));

        var response = await CreateSut().GetAllHotelsAsync();

        response.Errors.Should().ContainSingle(e => e.Message == "db down");
        response.Data.Should().BeNull();
    }
}

